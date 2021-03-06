﻿// ドロップデータ
namespace Entities {
    export class DropReward {
        idWithType: number; // ID
        num: number;        // 個数
    }

    export class Drop {
        bucket: Bucket<Drop>;
        private identify: Identify;
        private rewards: Identify[] = [];
        private weight: number[] = [];
        private num: number[] = [];
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
            this.push("報酬ID１", "重み１", "個数１");
            this.push("報酬ID２", "重み２", "個数２");
            this.push("報酬ID３", "重み３", "個数３");
        }

        private push(reward: string, weight: string, num: string) {
            let r = Identify.Parse(this.bucket.first.get(reward, ""));
            let w: number = this.bucket.first.get(weight, 0);
            let n: number = this.bucket.first.get(num, 1);
            // MEMO : ハズレが設定できるため isEmpty はチェックしない
            if (w <= 0) return;
            this.rewards.push(r);
            this.weight.push(w);
            this.num.push(n);
            this.weightTotal += w;
        }

        // 抽選
        draw(): DropReward[] {
            let res: DropReward[] = [];
            let count = this.drawCount;
            for (var i = 0; i < count; i++) {
                let index = Random.Weight(this.weight, this.weightTotal);
                let id = this.rewards[index];
                if (!Identify.isEmpty(id)) {
                    let item = new DropReward();
                    item.idWithType = id.idWithType;
                    item.num = this.num[index];
                    res.push(item);
                }
            }
            return res;
        }
    }
}
