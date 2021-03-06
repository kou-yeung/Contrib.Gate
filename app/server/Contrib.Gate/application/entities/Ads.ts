﻿// 広告
namespace Entities {

    export class Ads {
        bucket: Bucket<Ads>;
        user: KiiUser;
        constructor(admin: KiiAppAdminContext, user: KiiUser) {
            this.bucket = new Bucket(this, admin, "advertisement");
            this.user = user;
        }
        refresh(done: (ads: Ads) => void): void {
            let clause = KiiClause.equals("userid", this.user.getID());
            this.bucket.refresh(done, KiiQuery.queryWithClause(clause));
        }

        get userid(): string {
            return this.bucket.first.get("userid");
        };
        set userid(userid: string) {
            this.bucket.first.set("userid", userid);
        }
        get guid(): string {
            return this.bucket.first.get("guid");
        };
        set guid(guid: string) {
            this.bucket.first.set("guid", guid);
        }
        get reward(): AdReward {
            return this.bucket.first.get("reward");
        }
        set reward(reward: AdReward) {
            this.bucket.first.set("reward", reward);
        }
        get param(): string {
            return this.bucket.first.get("param");
        }
        set param(param: string) {
            this.bucket.first.set("param", param);
        }
        get date(): number {
            return this.bucket.first.get("date", Util.Time.ServerTime.current);
        }
        set date(date: number) {
            this.bucket.first.set("date", date);
        }

        create(reward: AdReward, param: string, done: (ads: Ads) => void) {
            this.userid = this.user.getID();
            this.guid = GUID.Gen();
            this.reward = reward;
            this.param = param;
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
            //return (Util.Time.ServerTime.current - this.date) >= 10;
            return (Util.Time.ServerTime.current - this.date) >= 0; // 検証で 0 にします。時間制限なし
        }
    }
}