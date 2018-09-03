// MEMO : KiiCloud では Arrayが使えないため extendsは 使用できません!!
// このクラスはメンバで持ちます
class Bucket<T> {
    private user: KiiUser;
    private t: T;
    private bucketName: string;
    private obj: KiiObject;

    constructor(t: T, user: KiiUser, bucketName: string) {
        this.t = t;
        this.user = user;
        this.bucketName = bucketName;
    }

    refresh(done: (t: T) => void): void {
        let self = this;

        var bucket = self.user.bucketWithName(self.bucketName);

        bucket.executeQuery(KiiQuery.queryWithClause(), {
            success: function (query, result, next) {
                if (result.length != 0) {
                    self.obj = result[0];
                } else {
                    self.obj = self.user.bucketWithName(self.bucketName).createObject();
                }
                done(self.t);
            },
            failure: function (query, error) {
                // エラー処理
            }
        });
    }

    save(done: (t: T) => void): void {
        let self = this;
        self.obj.save({
            success: function (theSavedObject: KiiObject) {
                done(self.t);
            },
            failure: function (theObject, error) {
            }
        }, true);
    };

    get<T>(key: string): T {
        return this.obj.get<T>(key);
    }
    set<T>(key: string, value: T) {
        this.obj.set<T>(key, value);
    }
    has(key: string): boolean {
        return this.obj.getKeys().indexOf(key) != -1;
    }
}
