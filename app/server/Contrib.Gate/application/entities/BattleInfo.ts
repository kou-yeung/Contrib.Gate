// ユーザが現在バトルしている情報
namespace Entities {
    export class BattleInfo {
        bucket: Bucket<BattleInfo>;

        constructor(user: KiiUser) {
            this.bucket = new Bucket(this, user, "battle_info");
        }
        refresh(done: (battleInfo: BattleInfo) => void) {
            this.bucket.refresh(done);
        }
        // 開始時のGUID
        get guid(): string {
            return this.bucket.first.get("guid");
        }
        set guid(guid: string) {
            this.bucket.first.set("guid", guid);
        }
        // ドロップテーブル
        get drop(): Identify {
            return Identify.Parse(this.bucket.first.get("drop"));
        }
        set drop(drop: Identify) {
            this.bucket.first.set("drop", drop.toString());
        }
        // 獲得お金
        get coin(): number {
            return this.bucket.first.get("coin");
        }
        set coin(coin: number) {
            this.bucket.first.set("coin", coin);
        }
        // ダンジョンにいるペットたち
        get pets(): string[] {
            return this.bucket.first.get("pets");
        }
        set pets(pets: string[]) {
            this.bucket.first.set("pets", pets);
        }
        get exps(): ExpItem[] {
            return this.bucket.first.get("exps");
        }
        set exps(items: ExpItem[]) {
            this.bucket.first.set("exps", items);
        }

        // 報酬一覧
        get rewards(): number[] {
            return this.bucket.first.get("rewards");
        }
        set rewards(rewards: number[]) {
            this.bucket.first.set("rewards", rewards);
        }
    }
}