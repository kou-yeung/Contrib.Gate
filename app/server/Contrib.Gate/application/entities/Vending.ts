// 自販機データ
namespace Entities {
    export class Vending {
        private bucket: Bucket<Vending>;
        private identify: Identify;

        constructor(admin: KiiAppAdminContext, identify: Identify) {
            this.bucket = new Bucket(this, admin, "vending");
            this.identify = identify;
        }

        refresh(done: (vending: Vending) => void) {
            let clause = KiiClause.equals("ID", this.identify.toString());
            this.bucket.refresh(done, KiiQuery.queryWithClause(clause));
        }

        get target(): Identify {
            return Identify.Parse(this.bucket.first.get("購入対象ID"));
        }
        get level(): number {
            return this.bucket.first.get("購入レベル");
        }
        get price(): number {
            return this.bucket.first.get("値段");
        }
        get num(): number {
            return this.bucket.first.get("個数");
        }
    }
}