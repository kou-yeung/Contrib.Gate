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
            this.bucket.refresh((r) => {
                done(r);
            }, KiiQuery.queryWithClause(clause));
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
            return this.bucket.get("date");
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
    }
}