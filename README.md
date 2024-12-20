## 1. Saga pattern, mikroservis mimarisinde şu temel sorunları çözmeye odaklanır:
- **Dağıtık İşlem Koordinasyonu:** Birden fazla servis arasındaki işlem sırasını düzenler.
- **Veri Tutarlılığı:** ACID yerine eventual consistency sağlar, başarısız işlemleri telafi eder.
- **Hata Yönetimi:** Başarısızlık durumunda rollback veya kompanzasyon mekanizmaları ile sorunları giderir.
- **Merkezi Olmayan Kontrol:** Merkezi bir yapıya bağlı olmadan işlem yönetimi sağlar.
- **Servis Bağımsızlığı:** Mikroservislerin birlikte çalışmasını destekler, bağımlılığı azaltır.
- **Performans ve Ölçeklenebilirlik:** Asenkron yapı ile performansı artırır ve sistemin ölçeklenmesini kolaylaştırır.

---

## 2.	Saga patterndeki choreography ve orchestration yaklaşımları arasındaki temel fark nedir?

### 1. Koordinasyon Yöntemi

- **Choreography:**
  - Her servis kendi işini tamamladıktan sonra bir event yayınlar.
  - Diğer servisler bu event'i dinleyerek kendi işlemlerini başlatır.
  - Dağıtık ve olay tabanlı bir yapıdır.

- **Orchestration:**
  - Bir merkezi Orchestrator tüm işlemleri koordine eder.
  - Her servise sırayla ne yapması gerektiğini söyler.
  - Merkezi kontrol söz konusudur.

### 2. Bağımlılık

- **Choreography:**
  - Daha gevşek bağlı bir yapıya sahiptir.
  - Servisler arısı doğrudan bağımlılık azalır.

- **Orchestration:**
  - Servisler merkezi bir orchestrator'a bağımlıdır.
  - Orchestrator olmadan işlem yönetimi gerçekleşmez.

### 3. Esneklik ve Bakım

- **Choreography:**
  - Daha esnektir, yeni bir servis eklemek daha kolaydır.
  - Ancak, karmaşık bir event zinciri oluşabilir ve takip zorlaşabilir.

- **Orchestration:**
  - Merkezi bir yapı sayesinde işlem akışı daha düzenlidir.
  - Ancak, orchestrator karmaşık hale gelirse bakım zorlaşabilir.

### 4. Performans

- **Choreography:**
  - Event bazlı asenkron iletişim olduğu için daha iyi performans sunar.

- **Orchestration:**
  - Merkezi orchestrator'ın yoğunluğu performansı sınırlayabilir.

---

## 3.	Orchestration Saga pattern avantajları ve dezavantajları nelerdir?

| Kriter            | Avantajlar                                                | Dezavantajlar                                                |
|--------------------|----------------------------------------------------------|--------------------------------------------------------------|
| **Kontrol**       | Merkezi bir orchestrator işlemleri düzenler ve yönetir. | Merkezi yapı, mikroservislerin bağımsızlığını kısmen sınırlandırabilir. |
| **İzlenebilirlik** | İşlem akışını izlemek ve sorunları tespit etmek kolaydır. | Orchestrator'ın izleme yetenekleri karmaşıklaşabilir.          |
| **Hata Yönetimi**  | Hatalar ve geri alma işlemleri merkezi olarak ele alınır. | Merkezi orchestrator'ın başarısızlığı tüm sistemi etkileyebilir.  |
| **Karmaşıklık**    | Karmaşık işlem akışları ve özel mantıklar kolayca yönetilebilir. | Orchestrator'ın kendisi çok karmaşık hale gelebilir.       |
| **Bağımlılık**    | Servisler süreci merkezi olarak koordine eder.        | Servisler orchestrator'a bağımlı hale gelir.                |
| **Performans**    | İşlem akışı düzenli ve sistematik hale gelir.           | Yoğun işlem trafiğinde orchestrator darboğaz yaratabilir.  |
| **Ölçeklenebilirlik**| Küçük ve orta sistemlerde daha kolay ölçeklenebilir.      | Büyük sistemlerde orchestrator yatay ölçeklenemez.         |
| **Tek Nokta Hatası**| Sorunlar merkezi olarak çözülür.                      | Orchestrator'ın çökmesi tüm işlemleri durdurabilir.        |

---

## 4.	Bir e-ticaret uygulaması tasarladığınızı düşünelim. Bu uygulamada müşteriler sipariş verdiklerinde, birden fazla hizmetin birlikte çalışması gerekiyor. Müşteri bir sipariş verdiğinde şu adımlar gerçekleşmeli:

Bir e-ticaret uygulamasında, müşteri bir sipariş verdiğinde şu adımların gerçekleşmesi gerekir:

1. Stokta mevcut ürünleri kontrol eder ve onları rezerve eder.
2. Müşterinin yeterli bakiye olup olmadığı kontrol edilir ve ödeme işlemi gerçekleştirilir. 
3.	Kargo ödeme onaylandıktan sonra gönderi için hazırlık yapar ve teslimat planlanır.

Burada dikkat etmeniz gereken bir nokta var: Eğer bu adımlardan herhangi biri başarısız olursa (örneğin, ödeme başarısız olursa veya stokta ürün yoksa), sistem önceki adımları geri alarak verilerin tutarlılığını sağlamalıdır. Yani, ödeme başarısız olursa stoktaki rezerv kaldırılmalı, kargo işlemi başarısız olursa ödeme iade edilmelidir..
Soru:
1.	Bu süreci yönetmek için bir Saga pattern tasarlayın ve basit bir durum makinesi (state machine) diyagramı çizin. Sipariş Verildi aşamasından Sipariş Tamamlandı aşamasına kadar olan her bir durumu çizin ve her bir başarısızlık durumunda geri alma adımlarını gösterin.

![1](https://github.com/user-attachments/assets/447238a9-99b8-43de-b9d9-c0420fafe651)

![2](https://github.com/user-attachments/assets/9459624f-1533-4d10-be85-973397aca4fe)

2.	Her bir durumda, ilgili hizmetin başarılı ya da başarısız olması durumunda nasıl bir geçiş yapılacağını açıklayın.

#### 1. Sipariş Hizmeti

- **Başarılı:**
  - Sipariş oluşturulur ve sistem bir sonraki adıma, yani Stok Hizmeti'ne geçer.
  - **Tetiklenen Olay:** “Sipariş oluşturuldu.”
  - **Koordinasyon:**
    - Olay tabanlı yaklaşımda "Sipariş oluşturuldu." mesajı Message Broker üzerinden Stok Hizmeti'ne gönderilir.
    - Orchestrator yaklaşımında, Stok Hizmeti'ni çağırır.
- **Başarısız:**
  - Sipariş oluşturulamazsa süreç burada sonlanır ve sipariş iptal edilir.
  - **Tetiklenen Olay:** “Sipariş iptal edildi.”
  - **Tutarlılık:** Hiçbir işlem yapılmadığı için rollback gerekmez.

#### 2. Stok Hizmeti

- **Başarılı:**
  - Stok rezerve edilir ve sistem bir sonraki adıma, yani Ödeme Hizmeti'ne geçer.
  - **Tetiklenen Olay:** “Stok rezerve edildi.”
  - **Koordinasyon:**
    - Olay tabanlı yaklaşımda Stok Hizmeti, "Stok rezerve edildi." mesajı Message Broker üzerinden Ödeme Hizmeti’ne gönderilir.
    - Orchestrator yaklaşımında, Ödeme Hizmeti'ni çağırır.
- **Başarısız:**
  - Stok rezervasyonu başarısız olursa sipariş iptal edilir.
  - **Tetiklenen Olay:** “Stok rezerve edilemedi.”
  - **Tutarlılık:** Sipariş Hizmeti'ne geri dönülür ve sipariş iptal edilir.
    - "Sipariş iptal edildi.”
    - Stok rezervasyonu yapılmadığı için başka rollback gerekmez.

#### 3. Ödeme Hizmeti

- **Başarılı:**
  - Ödeme başarıyla gerçekleştirilir ve sistem bir sonraki adıma, yani Kargo Hizmeti'ne geçer.
  - **Tetiklenen Olay:** “Ödeme tamamlandı.”
  - **Koordinasyon:**
    - Olay tabanlı yaklaşımda "Ödeme tamamlandı." mesajı Message Broker üzerinden Kargo Hizmeti'ne gönderilir.
    - Orchestrator yaklaşımında, Kargo Hizmeti'ni çağırır.
- **Başarısız:**
  - Ödeme başarısız olursa sistem önceki adıma döner.
  - **Tetiklenen Olay:** “Ödeme başarısız oldu.”
  - **Tutarlılık:**
    - "Stok rezerve edildi." geri alınır.
    - "Sipariş iptal edildi.”

#### 4. Kargo Hizmeti

- **Başarılı:**
  - Kargo gönderimi gerçekleştirilir ve süreç sona erer.
  - **Tetiklenen Olay:** “Kargo gönderildi.”
  - **Koordinasyon:**
    - Olay tabanlı yaklaşımda "Kargo gönderimi tamamlandı." mesajı Message Broker üzerinden sürecin sonlandığını bildirir.
    - Orchestrator süreci tamamlar ve sistemi sonlandırır.
- **Başarısız:**
  - Kargo gönderimi başarısız olursa sistem önceki adıma döner.
  - **Tetiklenen Olay:** “Kargo iptal oldu.”
  - **Tutarlılık:**
    - "Ödeme tamamlandı.” geri alınır.
    - "Stok rezerve edildi" geri alınır.
    - "Sipariş iptal edildi.”

---


