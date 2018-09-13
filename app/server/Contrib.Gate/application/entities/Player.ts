// Player データ
namespace Entities {

    export class Player
    {
        bucket: Bucket<Player>;
        constructor(user: KiiUser) {
            this.bucket = new Bucket(this, user, "Player");
        }

        get userCreateStep(): UserCreateStep {
            return this.bucket.get("UserCreateStep", UserCreateStep.EnterName);
        };
        set userCreateStep(step: UserCreateStep) {
            this.bucket.set("UserCreateStep", step);
        }

        get UserName(): string {
            return this.bucket.get("UserName");
        }
        set UserName(name: string) {
            this.bucket.set<string>("UserName", name);
        }

        get coin(): number {
            return this.bucket.get("coin", 0);
        }
        set coin(coin: number) {
            this.bucket.set("coin", coin);
        }
    }

}
