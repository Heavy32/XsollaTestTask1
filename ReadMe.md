Задания:
1) Должен быть метод, принимающий сумму и назначение платежа и возвращающий sessionId - идентификатором платёжной сессии, например, 4e273d86-864c-429d-879a-34579708dd69.
файл: https://github.com/Heavy32/XsollaTestTask1/blob/master/XsollaTestTask1/Controllers/PaymentSessionController.cs

2) Должен быть метод, который принимает данные карты (Номер, CVV/CVC, даты) и sessionId
файл: https://github.com/Heavy32/XsollaTestTask1/blob/master/XsollaTestTask1/Controllers/PaymentByCreditCardController.cs
метод: PayByCreditCard

3) Номер карты должен проверяться по алгоритму Луна (упрощенному). Валидные номера должны имитировать успешную оплату, невалидные — возвращать ошибку.
файл: https://github.com/Heavy32/XsollaTestTask1/blob/master/XsollaTestTask1/Models/CreditCard.cs
метод: Validate

4) Подготовить OpenAPI-спецификацию (и опубликовать её).
https://app.swaggerhub.com/apis/Heavy32/Xsollatasktest/1.0.0

5) Покрыть реализацию тестами.
файл: https://github.com/Heavy32/XsollaTestTask1/tree/master/XsollaTestTask1Tests

6) Ограничить время жизни платёжной сессии.
файл: https://github.com/Heavy32/XsollaTestTask1/blob/master/XsollaTestTask1/Controllers/PaymentByCreditCardController.cs
метод: PayByCreditCard, внутри проверка

7) Сделать Docker-образ, docker-compose файл
https://hub.docker.com/r/heavy32/xsollatesttask

8) Добавить API-метод, который возвращает список всех платежей за переданный период (должен быть закрыт авторизацией)
файл https://github.com/Heavy32/XsollaTestTask1/blob/master/XsollaTestTask1/Controllers/PaymentByCreditCardController.cs
метод GetAllPaymentHistory

9) После успешной оплаты асинхронно отправлять HTTP-уведомление на URL магазина (где URL -- параметр метода из п.2).
файл https://github.com/Heavy32/XsollaTestTask1/blob/master/XsollaTestTask1/Controllers/PaymentByCreditCardController.cs
метод SendNotificationToShop

Некоторые моменты из ТЗ были непонятны для меня.
"После успешной оплаты асинхронно отправлять HTTP-уведомление на URL магазина (где URL -- параметр метода из п.2)."
1) Мне кажется, что у вас здесь опечатка, и вы имели ввиду пункт 1. Либо вы случайно забыли сделать пунктом "Пусть это будет сервис с JSON API." (как это сделано во втором варианте). Я решил не отступать от написаного и сделать всё строго по ТЗ.
2) Возможно, вы имели ввиду "fire-and-forget", так как иначе при отправке запроса есть вероятность того, что весь метод оплаты зависнет в ожидании ответа от сервера
Однако я решил ничего не придумывать и сделать задание как понял. 
