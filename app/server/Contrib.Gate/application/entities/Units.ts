// ユニット一覧
namespace Entities {
    export class Units {
        bucket: Bucket<Units>;

        constructor(user: KiiUser) {
            this.bucket = new Bucket(this, user, "units");
        }

        refresh(done: (units: Units) => void) {
            //this.bucket.refresh(done);

            // 移行期間 : 初期化済かチェックする
            this.bucket.refresh((r) => {
                if (!r.bucket.first.has("items")) {
                    r.create();
                }
                done(r);
            });
        }

        create(): UnitItem[] {
            let items: UnitItem[] = [];
            for (var i = 0; i < Const.MaxUnit; i++) {
                let item = new UnitItem();
                item.expirationDate = (i < Const.FreeUnit) ? -1 : 0;
                item.id = i;
                item.name = "";
                item.uniqids = [];
                for (var j = 0; j < Const.MaxPetInUnit; j++) {
                    item.uniqids.push("");
                }
                items.push(item);
            }
            this.items = items;
            return items;
        }

        get items(): UnitItem[] {
            return this.bucket.first.get("items");
        }
        set items(units: UnitItem[]) {
            this.bucket.first.set("items", units);
        }

        // 指定ユニット番号にuniqidを追加します
        push(index: number, uniqid: string) {
            let items = this.items;
            for (var i = 0; i < Const.MaxPetInUnit; i++) {
                if (items[index].uniqids[i] != "") continue;
                items[index].uniqids[i] = uniqid;
                break;
            }
            this.items = items;
        }

        // 指定 uniqid が配置されています
        exists(uniqid: string): boolean {
            let items = this.items;
            for (var i = 0; i < Const.MaxUnit; i++) {

                for (var j = 0; j < Const.MaxPetInUnit; j++) {
                    if (items[i].uniqids[j] == uniqid) {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
