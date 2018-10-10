// タマゴデータ
namespace Entities {
    export class Egg {
        bucket: Bucket<Egg>;
        private guid: string;
        constructor(user: KiiUser, guid: string = null) {
            this.bucket = new Bucket(this, user, "egg");
            this.guid = guid;
        }

        refresh(done: (egg: Egg) => void) {
            if (this.guid == null) {
                // すべてを取得する
                this.bucket.refresh(done, KiiQuery.queryWithClause());
            } else {
                let clause = KiiClause.equals("uniqid", this.guid);
                this.bucket.refresh(done, KiiQuery.queryWithClause(clause));
            }
        }
        // uniqid (実質guid)
        get uniqid(): string {
            return this.bucket.first.get("uniqid");
        }
        set uniqid(uniqid: string) {
            this.bucket.first.set("uniqid", uniqid);
        }
        get item(): EggItem {
            return JSON.parse(this.bucket.first.get("item")) as EggItem;
        }
        set item(item: EggItem) {
            this.bucket.first.set("item", JSON.stringify(item));
        }
        // item のプレーンテキスト
        get plainText(): string {
            return this.bucket.first.get("item");
        }
        // リザルド
        get result(): Identify {
            return Identify.Parse(this.bucket.first.get("result"));
        }
        set result(result: Identify) {
            this.bucket.first.set("result", result.toString());
        }

        get valid(): boolean {
            return this.bucket.first.has("item");
        }
    }
}


// タマゴの追加：再帰
function InsertEggs(stage: number, user: KiiUser, eggs: Entities.Identify[], result: EggItem[], done) {
    // 再帰終了
    if (eggs.length <= 0) {
        done();
        return;
    }

    let id = eggs.shift();  // 先頭アイテムを取り出す
    let guid = GUID.Gen();  // 新たなGUIDを生成する
    new Entities.Egg(user).bucket.create(egg => {
        let item = new EggItem();
        item.createTime = Util.Time.ServerTime.current;
        item.uniqid = guid;
        item.judgment = false;
        item.stage = stage;
        egg.uniqid = guid;
        egg.item = item;
        egg.result = id;
        egg.bucket.save(() => {
            result.push(item);
            InsertEggs(stage, user, eggs, result, done);  // 次のタマゴを追加する
        });
    });
}
