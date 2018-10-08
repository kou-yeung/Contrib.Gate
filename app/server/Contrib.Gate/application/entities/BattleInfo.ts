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

        // ステージ
        get stage(): Identify {
            return Identify.Parse(this.bucket.first.get("stage"));
        }
        set stage(stage: Identify) {
            this.bucket.first.set("stage", stage.toString());
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

        // 獲得経験値
        get exp(): number {
            return this.bucket.first.get("exp");
        }
        set exp(exp: number) {
            this.bucket.first.set("exp", exp);
        }

        // 平均レベル
        get level(): number {
            return this.bucket.first.get("level");
        }
        set level(level: number) {
            this.bucket.first.set("level", level);
        }

        //get exps(): ExpItem[] {
        //    return this.bucket.first.get("exps");
        //}
        //set exps(items: ExpItem[]) {
        //    this.bucket.first.set("exps", items);
        //}

        // 報酬一覧
        get rewards(): DropReward[] {
            return this.bucket.first.get("rewards");
        }
        set rewards(rewards: DropReward[]) {
            this.bucket.first.set("rewards", rewards);
        }
    }
}