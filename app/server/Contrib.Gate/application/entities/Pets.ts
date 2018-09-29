// ペットデータ(ユーザ所持使い魔)
namespace Entities {
    export class Pets {
        bucket: Bucket<Pets>;

        constructor(user: KiiUser) {
            this.bucket = new Bucket(this, user, "pet");
        }

        refresh(guid: string[], done: (pets: Pets) => void) {
            let clause = KiiClause.equals("uniqid", guid[0]);
            for (var i = 1; i < guid.length; i++) {
                let add = KiiClause.equals("uniqid", guid[i]);
                clause = KiiClause.or(clause, add);
            }
            this.bucket.refresh(done, KiiQuery.queryWithClause(clause));
        }
        //// uniqid (実質guid)
        //get uniqid(): string {
        //    return this.bucket.first.get("uniqid");
        //}
        //set uniqid(uniqid: string) {
        //    this.bucket.first.set("uniqid", uniqid);
        //}
        //get item(): PetItem {
        //    return JSON.parse(this.bucket.first.get("item")) as PetItem;
        //}
        //set item(item: PetItem) {
        //    this.bucket.first.set("item", JSON.stringify(item));
        //}
        //// item のプレーンテキスト
        //get plainText(): string {
        //    return this.bucket.first.get("item");
        //}
        //get valid(): boolean {
        //    return this.bucket.first.has("item");
        //}
    }
}
