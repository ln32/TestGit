using System.Linq.Expressions;
using System.Reflection;

namespace CJH_Frame.CJH_FlagBaseTypeDictionary
{
    public class TypeBase_MultiValueDictionary<T> : Dictionary<string, List<T>>
    {
        /// <summary>
        /// 제네릭 메서드로 tryAdd를 구현합니다.
        /// Type은 T를 상속하거나 구현해야 합니다.
        /// 만약 해당 타입의 키가 이미 존재하면 false, 그렇지 않으면 추가 후 true를 반환합니다.
        /// </summary>
        /// 
        //public bool TryAdd<TType>(T target) where TType : T => TryAdd(typeof(TType).Name, target);
        public bool TryAdd(string typeName, T target)
        {
            // 키는 해당 타입의 전체 이름(FullName)을 사용합니다.
            string key = typeName;
            if (!ContainsKey(key))
                this[key] = new List<T>();

            this[key].Add(target);
            return true;
        }

        /// <summary>
        /// 제네릭 메서드로 tryAdd를 구현합니다.
        /// Type은 T를 상속하거나 구현해야 합니다.
        /// 만약 해당 타입의 키가 이미 존재하면 false, 그렇지 않으면 추가 후 true를 반환합니다.
        /// </summary>
        /// 
        public bool TryGetValue<TType>(out List<T>? target) where TType : T => TryGetValue(typeof(TType).Name, out target);

        /// <summary>
        /// 현재 딕셔너리의 모든 키(문자열)를 콘솔에 출력합니다.
        /// </summary>
        public void PrintAll()
        {
            foreach (string key in this.Keys)
            {
                Console.WriteLine(">> " + key);
            }
        }
    }
}



namespace CJH_Frame.CJH_ParamFactory
{
    public class Params : Dictionary<string, object> { }

    public class Builder
    {
        private readonly Params _params = new Params();

        public Builder Add(string key, object value)
        {
            _params[key] = value;
            return this;
        }

        public Builder Add<T>(object value)
        {
            _params[typeof(T).Name] = value;
            return this;
        }

        public void Clear()
        {
            _params.Clear();
        }

        public Params Build() => _params;
    }


    public static class ParamFactory<T>
    {
        private static Dictionary<string, Func<Params, T>> _skillCreators = new();

        public static void Init()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var types = assembly.GetTypes()
        .Where(t => typeof(T).IsAssignableFrom(t) && !t.IsAbstract)
        .ToList();

            foreach (var type in types)
            {
                var castingType = type.Name; // 타입 이름을 키로 사용
                var ctor = type.GetConstructors().First();

                // 컴파일된 람다 생성
                var paramExpr = Expression.Parameter(typeof(Params));
                var paramConverters = ctor.GetParameters().Select(p =>
                {
                    // SkillParams[p.Name]에 접근하는 식 트리 생성
                    var keyExpr = Expression.Constant(p.Name); // 파라미터 이름을 키로 사용
                    var valueExpr = Expression.MakeIndex(
                        paramExpr,
                        typeof(Params).GetProperty("Item"), // Item 프로퍼티 (인덱서)
                        new[] { keyExpr } // 키 값 전달
                    );

                    // object -> 실제 타입으로 변환
                    return Expression.Convert(valueExpr, p.ParameterType);
                }).ToArray();

                _skillCreators[castingType] = Expression.Lambda<Func<Params, T>>(
                    Expression.New(ctor, paramConverters),
                    paramExpr
                ).Compile();
            }
        }

        public static bool TryCreateSkill(string skillType, Params parameters, out T? result)
        {
            if (_skillCreators.TryGetValue(skillType, out var creator))
            {
                try
                {
                    result = creator(parameters);
                    return true;
                }
                catch (Exception e) { }
            }
            result = default;
            return false;
        }

        /*
        // 파라미터 유효성 검증 시스템
        public interface IParamValidator
        {
            bool Validate(SkillParams parameters);
        }

        // JSON 스키마 기반 검증 (Newtonsoft.Json.Schema 사용)
        public class JsonSchemaValidator// : IParamValidator
        {
            
            private readonly JSchema _schema;

            public JsonSchemaValidator(string schemaJson)
            {
                _schema = JSchema.Parse(schemaJson);
            }

            public bool Validate(SkillParams parameters)
            {
                var json = JObject.FromObject(parameters);
                return json.IsValid(_schema);
            }

            public void temp()
            {
                // 스킬별 검증기 등록
                JsonSchemaValidator _validators = new();
                _validators.Add("FireballSkill",
                    new JsonSchemaValidator(@"{
                'type': 'object',
                'properties': {
                    'Damage': {'type': 'integer', 'minimum': 1},
                    'Radius': {'type': 'number', 'exclusiveMinimum': 0}
                },
                'required': ['Damage']
            }"));

        */
        /*
            // 유니티 에디터에서의 사용 예
[SerializeField] private string _skillName;
[SerializeField] private List<SkillParamEntry> _params;

public void CreateInEditor()
{
    var builder = new SkillBuilder();
    foreach(var param in _params)
        builder.Add(param.Key, param.Value);
    TryCreateSkill(_skillName, builder.Build(), out _);
}
    }*/
    }
}