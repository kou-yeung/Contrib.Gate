// ペットデータ(ユーザ所持使い魔)
namespace Entities {
    export class Pet {
        bucket: Bucket<Pet>;
        private guid: string;

        constructor(user: KiiUser, guid: string = null) {
            this.bucket = new Bucket(this, user, "pet");
            this.guid = guid;
        }

        refresh(done: (pet: Pet) => void) {
            if (this.guid == null) {
                // すべてを取得する
                this.bucket.refresh(done, KiiQuery.queryWithClause());
            } else {
                let clause = KiiClause.equals("uniqid", this.guid);
                this.bucket.refresh(done, KiiQuery.queryWithClause(clause));
            }
        }

        // 使い魔ID
        get id(): Identify {
            return Identify.Parse(this.bucket.first.get("id"));
        }
        set id(id: Identify) {
            this.bucket.first.set("id", id.toString());
        }
        // uniqid (実質guid)
        get uniqid(): string {
            return this.bucket.first.get("uniqid");
        }
        set uniqid(uniqid: string) {
            this.bucket.first.set("uniqid", uniqid);
        }
        // 生成時間
        get createTime(): number {
            return this.bucket.first.get("createTime");
        }
        set createTime(createTime: number) {
            this.bucket.first.set("createTime", createTime);
        }
        // 経験値
        get exp(): number {
            return this.bucket.first.get("exp", 0);
        }
        set exp(createTime: number) {
            this.bucket.first.set("exp", createTime);
        }
        // 強化回数
        get powerupCount(): number {
            return this.bucket.first.get("powerupCount", 0);
        }
        set powerupCount(powerCount: number) {
            this.bucket.first.set("powerupCount", powerCount);
        }
        // パラメータ
        getParam(param: Param): number {
            return this.bucket.first.get(IDType[param], 0);
        }
        setParam(param: Param, value: number) {
            this.bucket.first.set(IDType[param], value);
        }
    }
}
