// 広告
namespace Entities {

    export class Ads {
        bucket: Bucket<Ads>;
        user: KiiUser;
        constructor(admin: KiiAppAdminContext, user: KiiUser) {
            this.bucket = new Bucket(this, admin, "Ads");
            this.user = user;
        }
        refresh(done: (ads: Ads) => void): void {
            let clause = KiiClause.equals("userid", this.user.getID());
            this.bucket.refresh(done, KiiQuery.queryWithClause(clause));
        }

        get userid(): string {
            return this.bucket.get("userid");
        };
        set userid(userid: string) {
            this.bucket.set("userid", userid);
        }
        get guid(): string {
            return this.bucket.get("guid");
        };
        set guid(guid: string) {
            this.bucket.set("guid", guid);
        }
        get reward(): AdReward {
            return this.bucket.get("reward");
        }
        set reward(reward: AdReward) {
            this.bucket.set("reward", reward);
        }
        get date(): number {
            return this.bucket.get("date", Util.Time.ServerTime.current);
        }
        set date(date: number) {
            this.bucket.set("date", date);
        }

        create(reward: AdReward, done: (ads: Ads) => void) {
            this.userid = this.user.getID();
            this.guid = GUID.Gen();
            this.reward = reward;
            this.date = Util.Time.ServerTime.current;
            this.bucket.save(done);
        }

        clear(done: (ads: Ads) => void) {
            this.guid = ""; // 空き文字列に設定する
            this.bucket.save(done);
        }

        // 有効かどうか
        // MEMO : コード発行一定期間後有効になる仕組み
        vaild(id: string): boolean {
            if (id != this.guid) return false;  // id が違う
            return (Util.Time.ServerTime.current - this.date) >= 10;
        }
    }
}