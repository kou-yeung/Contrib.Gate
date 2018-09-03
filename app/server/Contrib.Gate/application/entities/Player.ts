// Player データ
namespace Entities {

    export class Player
    {
        bucket: Bucket<Player>;
        constructor(user: KiiUser) {
            this.bucket = new Bucket<Player>(this, user, "Player");
        }

        get userCreateStep(): UserCreateStep {
            if (this.bucket.has("UserCreateStep")) {
                return this.bucket.get<UserCreateStep>("UserCreateStep");
            } else {
                return UserCreateStep.EnterName;
            }
        };
        set userCreateStep(step: UserCreateStep) {
            this.bucket.set("UserCreateStep", step);
        }

        get UserName(): string {
            return this.bucket.get<string>("UserName");
        }
        set UserName(name: string) {
            this.bucket.set<string>("UserName", name);
        }
    }

}