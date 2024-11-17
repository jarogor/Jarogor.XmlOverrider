# Jarogor.XmlOverrider

[RU] Пакет может помочь переопределить XML-документ другими документами с помощью простых правил.

[EN] Package can help you override an XML document with other documents using simple rules.

## Возможности

Правила перекрытия создаются по [xsd-схеме](./src/Jarogor.XmlOverrider/Scheme/Rules.xsd).

### Поиск
- По имени атрибута и его значению:
  - _Искать все элементы `qwerty` с атрибутом `foo` и значением `bar`_:
    ```xml
    <node name="qwerty" attributeIdName="foo" attributeIdValue="bar" />
    ```
- По имени атрибута.
  - _Искать все элементы `qwerty` с совпадающим значением атрибута `foo`_:
    ```xml
    <node name="qwerty" attributeIdName="foo" />
    ```

### Замена
- Только значения атрибута.
  - _Искать все элементы `qwerty` с совпадающим значением атрибута `foo` и заменить все значения у атрибута `bar`_:
    ```xml
    <node name="qwerty" attributeIdName="foo" override="attributes">
        <attribute name="bar"/>
    </node>
    ```
- Всего содержимого элемента.
  - _Искать все элементы `qwerty` с совпадающим значением атрибута `foo` и заменить всё содержимое элемента_:
    ```xml
    <node name="item" attributeIdName="key" attributeIdValue="b" override="innerXml"/>
    ```

## Пример использования

```csharp
// Добавление правил перекрытия (см. схему src/Jarogor.XmlOverrider/Scheme/Rules.xsd):
//  - для элемента 'bar' внтутри элемента 'foo',
//  - найти соответствия по значению атрибута 'key'
//  - и заменить значение атрибута 'value'
var rulesStream = new StringReader(
    """
    <overrideRules>
        <node name="foo">
            <node name="bar" attributeIdName="key" override="attributes">
                <attribute name="value"/>
            </node>
        </node>
    </overrideRules>
    """);

// Создание оверайдера и добавление исходного, перекрываемого xml
var overrider = new StringOverrider(
    new Rules(rulesStream),
    """
    <root><foo><bar key="a" value="1"/></foo></root>
    """);

// Добавление перекрывающего xml
overrider.AddOverride(
    """
    <root><foo><bar key="a" value="2"/></foo></root>
    """);

// Запуск обработки
var actual = overrider.Processing().Get();

// Результат
Console.WriteLine(actual.OuterXml);
// <root><foo><bar key="a" value="2" /></foo></root>
```