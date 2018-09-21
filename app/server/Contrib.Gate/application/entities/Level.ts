// インベントリデータ
namespace Entities {
    export class Level {
        private bucket: Bucket<Level>;

        constructor(admin: KiiAppAdminContext) {
            this.bucket = new Bucket(this, admin, "level");
        }

        refresh(done: (level: Level) => void) {
            this.bucket.refresh(done);
        }

        // exp -> level を計算する
        level(exp: number): number {
            let table = this.bucket.first.get<number[]>("exp");
            for (var i = 0; i < table.length; i++) {
                if (exp < table[i]) return i;
            }
            return table.length;
        }

        // level -> exp を計算する
        exp(level: number): number {
            let table = this.bucket.first.get<number[]>("exp");
            var index = Math.min(table.length - 1, level - 1);
            return table[index];
        }
    }
}
