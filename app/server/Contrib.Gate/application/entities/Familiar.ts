// 使い魔データ(CSVデータ)
namespace Entities {
    export class Familiar {
        bucket: Bucket<Familiar>;
        identify: Identify;
        constructor(admin: KiiAppAdminContext, identify: Identify) {
            this.bucket = new Bucket(this, admin, "familiar");
            this.identify = identify;
        }

        refresh(done: (familiar: Familiar) => void) {
            let clause = KiiClause.equals("ID", this.identify.toString());
            this.bucket.refresh(done, KiiQuery.queryWithClause(clause));
        }
        // 種族
        get race(): Race {
            return this.bucket.first.get("種族");
        }
        // レアリティ
        get rarity(): number {
            return this.bucket.first.get("レアリティ");
        }
    }
}
