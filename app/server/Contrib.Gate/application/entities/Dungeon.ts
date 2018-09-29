// ダウンジョンデータ
namespace Entities {
    export class Dungeon {
        bucket: Bucket<Dungeon>;
        identify: Identify;

        groud: Identify[] = [];   // グループID
        weight: number[] = [];    // 重み
        weightTotal: number = 0;      // 重み合計

        constructor(admin: KiiAppAdminContext, identify: Identify) {
            this.bucket = new Bucket(this, admin, "dungeon");
            this.identify = identify;
        }

        refresh(done: (dungeon: Dungeon) => void) {
            let clause = KiiClause.equals("ID", this.identify.toString());
            this.bucket.refresh((r) => { r.init(); done(r); }, KiiQuery.queryWithClause(clause));
        }

        private init() {
            this.push("敵１", "出現重み１");
            this.push("敵２", "出現重み２");
            this.push("敵３", "出現重み３");
        }

        private push(groud: string, weight: string) {
            let g = Identify.Parse(this.bucket.first.get(groud, ""));
            let w = this.bucket.first.get(weight, 0);
            if (Identify.isEmpty(g) || w <= 0) return;
            this.groud.push(g);
            this.weight.push(w);
            this.weightTotal += w;
        }

        get up(): Identify {
            return Identify.Parse(this.bucket.first.get("上り階段"));
        }
        get down(): Identify {
            return Identify.Parse(this.bucket.first.get("下り階段"));
        }

        get randomGroud(): Identify {
            let index = Random.Weight(this.weight, this.weightTotal);
            return this.groud[index];
        }
    }
}
