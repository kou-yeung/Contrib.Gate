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
        // 勝利した場合の獲得経験値
        get exp(): number {
            return this.bucket.first.get("exp");
        }
        set exp(exp: number) {
            this.bucket.first.set("exp", exp);
        }
        // ドロップテーブル
        get drop(): Identify {
            return Identify.Parse(this.bucket.first.get("drop"));
        }
        set drop(drop: Identify) {
            this.bucket.first.set("drop", drop.toString());
        }
    }
}