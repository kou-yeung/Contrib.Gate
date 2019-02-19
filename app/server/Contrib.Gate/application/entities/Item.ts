//// アイテムデータ
//namespace Entities {

//    export class ItemEffect {
//        param: Param[] = [];    // パラメータ
//        value: number[] = [];   // 効果
//        powerup: number = 0;    // 満腹度の増加
//    }

//    export class Item {
//        bucket: Bucket<Item>;
//        effects: { [id: number]: ItemEffect; } = [];

//        constructor(admin: KiiAppAdminContext) {
//            this.bucket = new Bucket(this, admin, "item");
//        }

//        refresh(identify: Identify[], done: (item: Item) => void) {
//            if (identify.length == 0) {
//                this.bucket.refresh(done);
//            } else {
//                let clause = KiiClause.equals("ID", identify[0].toString());
//                for (var i = 1; i < identify.length; i++) {
//                    let add = KiiClause.equals("ID", identify[i].toString());
//                    clause = KiiClause.or(clause, add);
//                }
//                this.bucket.refresh(r => { r.init(); done(r); }, KiiQuery.queryWithClause(clause));
//            }
//        }

//        private init() {
//            let results = this.bucket.results;
//            results.forEach(result => {
//                let id = Identify.Parse(result.get("ID"));
//                let effects = new ItemEffect();
//                effects.powerup = result.get("餌増加");
//                this.push(result, effects, "対象パラメータ１", "増減１");
//                this.push(result, effects, "対象パラメータ２", "増減２");

//                this.effects[id.idWithType] = effects;
//            });
//        }

//        private push(result: Result, effects: ItemEffect, param: string, value: string) {
//            let p = result.get(param, "");
//            let v: number = result.get(value, 0);
//            if (p == "" || v <= 0) return;
//            effects.param.push(Param[p]);
//            effects.value.push(v);
//        }

//        find(identify: number): ItemEffect {
//            return this.effects[identify];
//        }
//    }
//}
