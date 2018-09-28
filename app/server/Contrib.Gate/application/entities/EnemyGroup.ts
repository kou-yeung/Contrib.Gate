// 敵グループ(CSVデータ)
namespace Entities {
    export class EnemyGroud {
        bucket: Bucket<EnemyGroud>;
        identify: Identify;
        enemies: Identify[] = [];
        ranges: Range[] = [];

        constructor(admin: KiiAppAdminContext, identify: Identify) {
            this.bucket = new Bucket(this, admin, "enemy_group");
            this.identify = identify;
        }

        refresh(done: (enemyGroud: EnemyGroud) => void) {
            let clause = KiiClause.equals("ID", this.identify.toString());
            this.bucket.refresh((r) => { r.init(); done(r); }, KiiQuery.queryWithClause(clause));
        }

        private init() {
            this.push("敵１", "レベル範囲１");
            this.push("敵２", "レベル範囲２");
            this.push("敵３", "レベル範囲３");
            this.push("敵４", "レベル範囲４");
        }

        private push(enemy: string, level: string) {
            let e = Identify.Parse(this.bucket.first.get(enemy, ""));
            let l = Range.Parse(this.bucket.first.get(level, ""));
            if (Identify.isEmpty(e) || !l.vaild) return;
            this.enemies.push(e);
            this.ranges.push(l);
        }
    }
}
