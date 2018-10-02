// ユニット一覧
namespace Entities {
    export class Units {
        private bucket: Bucket<Units>;

        constructor(user: KiiUser) {
            this.bucket = new Bucket(this, user, "units");
        }

        refresh(done: (units: Units) => void) {
            //this.bucket.refresh(done);

            // 移行期間 : 初期化済かチェックする
            this.bucket.refresh((r) => {
                if (!r.bucket.first.has("item")) {
                    r.create();
                }
                done(r);
            });
        }

        create(): UnitItem[] {
            let units: UnitItem[] = [];
            for (var i = 0; i < Const.MaxUnit; i++) {
                let item = new UnitItem();
                item.expirationDate = (i < Const.FreeUnit) ? -1 : 0;
                item.name = "";
                item.uniqids = [];
                for (var j = 0; j < Const.MaxPetInUnit; j++) {
                    item.uniqids.push("");
                }
                units.push(item);
            }
            this.units = units;
            return units;
        }

        get units(): UnitItem[] {
            return this.bucket.first.get("item");
        }
        set units(units: UnitItem[]) {
            this.bucket.first.set("item", units);
        }
    }
}
