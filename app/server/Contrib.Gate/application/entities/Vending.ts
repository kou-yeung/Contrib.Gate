// 自販機データ
namespace Entities {
    export class Vending {
        bucket: Bucket<Vending>;
        identify: Identify;

        constructor(admin: KiiAppAdminContext, identify: Identify) {
            this.bucket = new Bucket(this, admin, "vending");
            this.identify = identify;
        }

        refresh(done: (vending: Vending) => void) {
            let clause = KiiClause.equals("購入対象ID", this.identify.toString());
            this.bucket.refresh(done, KiiQuery.queryWithClause(clause));
        }

        get id(): string {
            return this.bucket.get("購入対象ID");
        }

        get target(): Identify {
            return Identify.Parse(this.bucket.get("購入対象ID"));
        }
        get level(): number {
            return this.bucket.get("購入レベル");
        }
        get price(): number {
            return this.bucket.get("値段");
        }
        get num(): number {
            return this.bucket.get("個数");
        }
    }
}