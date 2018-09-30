// ドロップデータ
namespace Entities {
    export class Drop {
        bucket: Bucket<Drop>;
        private identify: Identify;
        private rewards: Identify[] = [];
        private weight: number[] = [];
        private weightTotal: number = 0;      // 重み合計

        constructor(admin: KiiAppAdminContext, identify: Identify) {
            this.bucket = new Bucket(this, admin, "drop");
            this.identify = identify;
        }

        refresh(done: (drop: Drop) => void) {
            let clause = KiiClause.equals("ID", this.identify.toString());
            this.bucket.refresh((r) => { r.init(); done(r); }, KiiQuery.queryWithClause(clause));
        }
        // 抽選回数
        private get drawCount(): number {
            return this.bucket.first.get("抽選回数");
        }

        private init() {
            this.push("報酬ID１", "重み１");
            this.push("報酬ID２", "重み２");
            this.push("報酬ID３", "重み３");
        }

        private push(reward: string, weight: string) {
            let r = Identify.Parse(this.bucket.first.get(reward));
            let w: number = this.bucket.first.get(weight);
            // MEMO : ハズレが設定できるため isEmpty はチェックしない
            if (w <= 0) return;
            this.rewards.push(r);
            this.weight.push(w);
            this.weightTotal += w;
        }

        // 抽選
        draw(): Identify[] {
            let res: Identify[] = [];
            let count = this.drawCount;
            for (var i = 0; i < count; i++) {
                let index = Random.Weight(this.weight, this.weightTotal);
                let id = this.rewards[index];
                if (!Identify.isEmpty(id)) {
                    res.push(id);
                }
            }
            return res;
        }
    }
}
