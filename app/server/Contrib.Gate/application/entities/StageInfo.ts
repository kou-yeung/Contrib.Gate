// ユーザが遊んでるステージ情報
namespace Entities {
    export class StageInfo {
        bucket: Bucket<StageInfo>;

        constructor(user: KiiUser) {
            this.bucket = new Bucket(this, user, "stage_info");
        }
        refresh(done: (stageInfo: StageInfo) => void) {
            this.bucket.refresh(done);
        }
        get guid(): string {
            return this.bucket.first.get("guid");
        }
        set guid(guid:string) {
            this.bucket.first.set("guid", guid);
        }
        get dungeon(): Identify {
            return Identify.Parse(this.bucket.first.get("dungeon"));
        }
        set dungeon(id: Identify) {
            this.bucket.first.set("dungeon", id.toString());
        }

        // 消失時間
        get lossTime(): number {
            return this.bucket.first.get("lossTime");
        }
        set lossTime(lossTime: number) {
            this.bucket.first.set("lossTime", lossTime);
        }
    }
}