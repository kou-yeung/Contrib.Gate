// Player データ
namespace Entities {

    export class Player
    {
        bucket: Bucket<Player>;
        constructor(user: KiiUser) {
            this.bucket = new Bucket(this, user, "player");
        }

        get userCreateStep(): UserCreateStep {
            return this.bucket.first.get("UserCreateStep", UserCreateStep.EnterName);
        };
        set userCreateStep(step: UserCreateStep) {
            this.bucket.first.set("UserCreateStep", step);
        }

        get UserName(): string {
            return this.bucket.first.get("UserName");
        }
        set UserName(name: string) {
            this.bucket.first.set<string>("UserName", name);
        }

        get coin(): number {
            return this.bucket.first.get("coin", 0);
        }
        set coin(coin: number) {
            this.bucket.first.set("coin", coin);
        }

        get userState(): UserState {
            let userState = new UserState();
            userState.createStep = this.userCreateStep;
            userState.playerName = this.UserName;
            userState.coin = this.coin;
            return userState;
        }
    }

}
