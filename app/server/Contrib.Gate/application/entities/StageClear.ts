// ステージクリア情報
namespace Entities {
    export class StageClear {
        bucket: Bucket<StageClear>;

        constructor(user: KiiUser) {
            this.bucket = new Bucket(this, user, "stage_clear");
        }
        refresh(done: (stageClear: StageClear) => void) {
            this.bucket.refresh(done);
        }
        get item(): StageItem[] {
            return this.bucket.first.get("item", []);
        }
        set item(item: StageItem[]) {
            this.bucket.first.set("item", item);
        }

        clear(stage: Identify): StageItem {
            let id = stage.idWithType;
            let item = this.item;
            for (var i = 0; i < item.length; i++) {
                if (item[i].id != id) continue;
                item[i].clear = Util.Time.ServerTime.current;
                this.item = item;
                return item[i];
            }
            // ここまでくる場合新規追加
            let data = new StageItem();
            data.id = id;
            data.clear = Util.Time.ServerTime.current;
            item.push(data);
            this.item = item;
            return data;
        }
    }
}
