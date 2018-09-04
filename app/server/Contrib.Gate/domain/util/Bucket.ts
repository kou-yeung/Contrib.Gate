// MEMO : KiiCloud では Arrayが使えないため extendsは 使用できません!!
// このクラスはメンバで持ちます
class Bucket<T> {
    private user: KiiUser | KiiAppAdminContext;
    private t: T;
    private bucketName: string;
    private obj: KiiObject;

    constructor(t: T, user: KiiUser | KiiAppAdminContext | null, bucketName: string) {
        this.t = t;
        this.user = user;
        this.bucketName = bucketName;
    }

    private bucketWithName(): KiiBucket {
        if (this.user == null) {
            return Kii.bucketWithName(this.bucketName);
        } else {
            return this.user.bucketWithName(this.bucketName);
        }
    }
    refresh(done: (t: T) => void): void {
        let self = this;

        var bucket = self.bucketWithName();

        bucket.executeQuery(KiiQuery.queryWithClause(), {
            success: function (query, result, next) {
                if (result.length != 0) {
                    self.obj = result[0];
                } else {
                    self.obj = self.bucketWithName().createObject();
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

    get<T>(key: string, def?: T): T {
        if (def == undefined || this.has(key)) {
            return this.obj.get<T>(key);
        } else {
            return def;
        }
    }
    set<T>(key: string, value: T) {
        this.obj.set<T>(key, value);
    }
    has(key: string): boolean {
        return this.obj.getKeys().indexOf(key) != -1;
    }
}
