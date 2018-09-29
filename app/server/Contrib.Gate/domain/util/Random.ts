class Random {

    // [min,max)
    static NextInteger(min: number, max: number): number {
        return Math.floor(this.NextDouble(min,max));
    }

    // [min,max)
    static NextDouble(min: number, max: number): number {
        return Math.random() * (max - min) + min;
    }

    // 重みによる抽選 : 配列のIndex
    static Weight(weight: number[], total?: number): number {
        // 合計を計算する
        if (total == undefined || total == NaN) {
            total = 0;
            for (var i = 0; i < weight.length; i++) {
                total += weight[i];
            }
        }
        // 実際の抽選
        let random = Random.NextInteger(0, total);
        for (var i = 0; i < weight.length; i++) {
            random -= weight[i];
            if (random < 0) return i;
        }
        return weight.length - 1;   // エラー対策として、末尾の返す

    }
}
