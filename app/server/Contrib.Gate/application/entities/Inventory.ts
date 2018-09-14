// 自販機データ
namespace Entities {
    export class Inventory {
        bucket: Bucket<Inventory>;

        constructor(user: KiiUser) {
            this.bucket = new Bucket(this, user, "inventory");
        }

        refresh(done: (inventory: Inventory) => void) {
            this.bucket.refresh(done, KiiQuery.queryWithClause());
        }

        num(identify: Identify): number {
            return this.bucket.first.get(identify.toString(), 0);
        }

        add(identify: Identify, add: number): number {
            let res = this.num(identify) + add;
            this.bucket.first.set(identify.toString(), res);
            return res;
        }
    }
}
