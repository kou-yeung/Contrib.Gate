// ダウンジョンデータ
namespace Entities {
    export class Dungeon {
        bucket: Bucket<Dungeon>;
        identify: Identify;

        constructor(admin: KiiAppAdminContext, identify: Identify) {
            this.bucket = new Bucket(this, admin, "dungeon");
            this.identify = identify;
        }

        refresh(done: (dungeon: Dungeon) => void) {
            let clause = KiiClause.equals("ID", this.identify.toString());
            this.bucket.refresh(done, KiiQuery.queryWithClause(clause));
        }

        get up(): Identify {
            return Identify.Parse(this.bucket.first.get("上り階段"));
        }
        get down(): Identify {
            return Identify.Parse(this.bucket.first.get("下り階段"));
        }
    }
}
