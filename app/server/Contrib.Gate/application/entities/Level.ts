// インベントリデータ
namespace Entities {
    export class Level {
        private bucket: Bucket<Level>;
        private table: number[];

        constructor(admin: KiiAppAdminContext) {
            this.bucket = new Bucket(this, admin, "level");
        }

        refresh(done: (level: Level) => void) {
            this.bucket.refresh((r) => { r.init(); done(r); });
        }

        private init() {
            this.table = this.bucket.first.get<number[]>("exp");
        }
        // exp -> level を計算する
        // start は検索開始レベル.ループ回数を減らす対策
        level(exp: number, start: number = 0): number {
            for (var i = start; i < this.table.length; i++) {
                if (exp < this.table[i]) return i;
            }
            return this.table.length;
        }

        // level -> exp を計算する
        exp(level: number): number {
            var index = Math.min(this.table.length - 1, level - 1);
            return this.table[index];
        }
    }
}
