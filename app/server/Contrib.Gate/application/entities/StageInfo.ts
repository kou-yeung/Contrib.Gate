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
        // ステージID
        get stage(): Identify {
            return Identify.Parse(this.bucket.first.get("stage"));
        }
        set stage(identify: Identify) {
            this.bucket.first.set("stage", identify.toString());
        }
        // 乱数シード値
        get seed(): number {
            return this.bucket.first.get("seed");
        }
        set seed(seed: number) {
            this.bucket.first.set("seed", seed);
        }
        // 開始時のGUID
        get guid(): string {
            return this.bucket.first.get("guid");
        }
        set guid(guid:string) {
            this.bucket.first.set("guid", guid);
        }
        // 現在のダンジョンID
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

        // 有効性をチェック
        get vaild(): boolean {
            return this.lossTime >= Util.Time.ServerTime.current;
        }

        // ダンジョンにいるペットたち
        get pets(): string[] {
            return this.bucket.first.get("pets");
        }
        set pets(pets: string[]) {
            this.bucket.first.set("pets", pets);
        }
    }
}