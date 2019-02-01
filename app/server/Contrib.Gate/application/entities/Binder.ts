// 図鑑データ
namespace Entities {
    export class Binder {
        bucket: Bucket<Binder>;

        constructor(user: KiiUser) {
            this.bucket = new Bucket(this, user, "binder");
        }

        refresh(done: (binder: Binder) => void) {
            this.bucket.refresh(done);
        }

        add(identify: Identify) {
            this.bucket.first.set(identify.toString(), 1);
        }
    }
}