// ペットデータ(ユーザ所持使い魔)
namespace Entities {
    export class Pet {
        bucket: Bucket<Pet>;
        private guid: string;

        constructor(user: KiiUser, guid: string = null) {
            this.bucket = new Bucket(this, user, "pet");
            this.guid = guid;
        }

        refresh(done: (pet: Pet) => void) {
            if (this.guid == null) {
                // すべてを取得する
                this.bucket.refresh(done, KiiQuery.queryWithClause());
            } else {
                let clause = KiiClause.equals("uniqid", this.guid);
                this.bucket.refresh(done, KiiQuery.queryWithClause(clause));
            }
        }
        create(done: (pet: Pet) => void) {
            this.bucket.create(done);
        }

        // uniqid (実質guid)
        get uniqid(): string {
            return this.bucket.first.get("uniqid");
        }
        set uniqid(uniqid: string) {
            this.bucket.first.set("uniqid", uniqid);
        }

        get item(): PetItem {
            return JSON.parse(this.bucket.first.get("item")) as PetItem;
        }
        set item(item: PetItem) {
            this.bucket.first.set("item", JSON.stringify(item));
        }
        // item のプレーンテキスト
        get plainText(): string {
            return this.bucket.first.get("item");
        }
        get valid(): boolean {
            return this.bucket.first.has("item");
        }

        gen(identify: Identify, level: number = 1, exp: number = 0): PetItem {

            let item = new PetItem();
            item.id = identify.idWithType;
            item.uniqid = GUID.Gen();
            item.createTime = Util.Time.ServerTime.current;
            item.level = level;
            item.exp = exp;
            item.powerupCount = 0;
            item.skill = 0;
            item.param = [];
            for (var i = 0; i < Score.Count; i++) item.param.push(0);
            this.item = item;
            this.uniqid = item.uniqid;
            return item;
        }
    }
}
