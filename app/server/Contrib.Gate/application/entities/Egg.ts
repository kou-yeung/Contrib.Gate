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

        // 種族
        get race(): Race {
            return this.bucket.first.get("race");
        }
        set race(race: Race) {
            this.bucket.first.set("race", race);
        }

        // レアリティ
        get rarity(): number {
            return this.bucket.first.get("rarity");
        }
        set rarity(rarity: number) {
            this.bucket.first.set("rarity", rarity);
        }
        // リザルド
        get result(): Identify {
            return Identify.Parse(this.bucket.first.get("result"));
        }
        set result(result: Identify) {
            this.bucket.first.set("result", result.toString());
        }

        // uniqid (実質guid)
        get uniqid(): string {
            return this.bucket.first.get("uniqid");
        }
        set uniqid(uniqid: string) {
            this.bucket.first.set("uniqid", uniqid);
        }
    }
}
