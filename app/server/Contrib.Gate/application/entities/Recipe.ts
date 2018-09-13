// 製作レシピ
namespace Entities {
    export class Recipe {
        private bucket: Bucket<Recipe>;
        private identify: Identify;
        material: Identify[] = [];
        num: number[] = [];

        constructor(admin: KiiAppAdminContext, identify: Identify) {
            this.bucket = new Bucket(this, admin, "recipe");
            this.identify = identify;
        }

        refresh(done: (recipe: Recipe) => void) {
            let clause = KiiClause.equals("ID", this.identify.toString());
            this.bucket.refresh((r) => { r.init(); done(r); }, KiiQuery.queryWithClause(clause));
        }

        private init() {
            this.push("素材１", "数１");
            this.push("素材２", "数２");
            this.push("素材３", "数３");
            this.push("素材４", "数４");
        }

        private push(material: string, num: string) {
            let m = Identify.Parse(this.bucket.get(material, ""));
            let n = this.bucket.get(num, 0);
            if (Identify.isEmpty(m) || n <= 0) return;
            this.material.push(m);
            this.num.push(n);
        }

        get count(): number {
            return this.material.length;
        }

        get result(): Identify {
            return Identify.Parse(this.bucket.get("Result"));
        }
    }
}