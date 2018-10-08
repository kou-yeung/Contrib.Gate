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
    }
}
