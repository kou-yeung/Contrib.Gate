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
    }
}
