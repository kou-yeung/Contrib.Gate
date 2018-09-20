namespace Entities {
    export class Stage {
        private bucket: Bucket<Stage>;
        private identify: Identify;

        constructor(admin: KiiAppAdminContext, identify: Identify) {
            this.bucket = new Bucket(this, admin, "stage");
            this.identify = identify;
        }

        refresh(done: (stage: Stage) => void) {
            let clause = KiiClause.equals("ID", this.identify.toString());
            this.bucket.refresh(done, KiiQuery.queryWithClause(clause));
        }

        get dungeon(): Identify {
            return Identify.Parse(this.bucket.first.get("Dungeon"));
        }
    }
}