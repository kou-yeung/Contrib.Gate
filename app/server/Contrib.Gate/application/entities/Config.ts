// Config
namespace Entities {
    export class Config {
        bucket: Bucket<Config>;
        private cryptKeys: number[];
        
        constructor(admin: KiiAppAdminContext) {
            this.bucket = new Bucket(this, admin, "config");
        }
        refresh(done: (config: Config) => void) {
            this.bucket.refresh(done);
        }

        set crypt(timestamp: number) {
            this.cryptKeys = [];
            // 末尾 3 桁を取得する
            let s = (timestamp % 1000).toString();
            for (var i = s.length - 1; i >= 0; i--) {
                this.cryptKeys.push(s.charCodeAt(i));
            }
        }
        get iv(): number[] {
            return this.encrypt(this.bucket.first.get("暗号化IV", ""));
        }
        get key(): number[] {
            return this.encrypt(this.bucket.first.get("暗号化KEY", ""));
        }

        encrypt(s: string): number[] {
            let res = [];
            for (var i = 0; i < s.length; i++)
            {
                res.push(s.charCodeAt(i) ^ this.cryptKeys[i % this.cryptKeys.length]);
            }
            return res;
        }
    }
}