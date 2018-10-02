// 使い魔データ(CSVデータ)
namespace Entities {
    export class Familiar {
        bucket: Bucket<Familiar>;
        private map: { [key: number]: Result; } = {};

        constructor(admin: KiiAppAdminContext) {
            this.bucket = new Bucket(this, admin, "familiar");
        }

        refresh(identify: Identify[], done: (familiar: Familiar) => void) {
            if (identify.length == 0) {
                this.bucket.refresh(done);
            } else {
                let clause = KiiClause.equals("ID", identify[0].toString());
                for (var i = 1; i < identify.length; i++) {
                    let add = KiiClause.equals("ID", identify[i].toString());
                    clause = KiiClause.or(clause, add);
                }
                this.bucket.refresh(r => { r.init(); done(r); }, KiiQuery.queryWithClause(clause));
            }
        }

        private init() {
            let results = this.bucket.results;
            results.forEach(result => {
                let id = Identify.Parse(result.get("ID"));
                this.map[id.idWithType] = result;
            });
        }
        // 種族
        get race(): Race {
            return this.bucket.first.get("種族");
        }
        // レアリティ
        get rarity(): number {
            return this.bucket.first.get("レアリティ");
        }

        find(identify: Identify): Result {
            return this.map[identify.idWithType];
        }
    }
}
