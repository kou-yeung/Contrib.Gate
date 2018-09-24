// 孵化データ
namespace Entities {
    export class Hatch {
        bucket: Bucket<Hatch>;
        private guid: string;

        constructor(user: KiiUser, guid: string = null) {
            this.bucket = new Bucket(this, user, "hatch");
            this.guid = guid;
        }

        refresh(done: (pet: Hatch) => void) {
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
        get item(): HatchItem {
            return JSON.parse(this.bucket.first.get("item")) as HatchItem;
        }
        set item(item: HatchItem) {
            this.bucket.first.set("item", JSON.stringify(item));
        }
        get valid(): boolean {
            return this.bucket.first.has("item");
        }
    }
}
