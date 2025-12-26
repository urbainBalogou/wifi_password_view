# 📚 Cours Professionnel Complet - WiFi Security & C# MAUI

> **Guide de préparation pour entretien technique**
> Maîtrise complète des concepts WiFi, C#, MAUI et architecture logicielle

---

## Table des Matières

1. [Partie I - Sécurité WiFi](#partie-i---sécurité-wifi)
2. [Partie II - C# et .NET](#partie-ii---c-et-net)
3. [Partie III - .NET MAUI](#partie-iii---net-maui)
4. [Partie IV - Architecture MVVM](#partie-iv---architecture-mvvm)
5. [Partie V - Concepts Avancés](#partie-v---concepts-avancés)
6. [Partie VI - Questions d'Entretien](#partie-vi---questions-dentretien)

---

# PARTIE I - SÉCURITÉ WIFI

## 1. Fondamentaux des Réseaux WiFi

### 1.1 Qu'est-ce que le WiFi ?

**Définition :**
WiFi (Wireless Fidelity) est une technologie de réseau sans fil basée sur la norme **IEEE 802.11**.

**Caractéristiques clés :**
- Fréquences : **2.4 GHz** et **5 GHz** (+ 6 GHz pour WiFi 6E)
- Portée : 30-50m en intérieur, jusqu'à 100m en extérieur
- Débits : de 11 Mbps (802.11b) à 9.6 Gbps (802.11ax/WiFi 6)

### 1.2 Architecture WiFi

```
[Appareil Client] <---> [Point d'Accès (AP)] <---> [Routeur] <---> [Internet]
     (STA)                    (AP)                  (Gateway)
```

**Composants :**
- **SSID** (Service Set Identifier) : Nom du réseau
- **BSSID** (Basic Service Set Identifier) : Adresse MAC du point d'accès
- **Canal** : Fréquence spécifique (1-14 pour 2.4GHz, 36-165 pour 5GHz)

### 1.3 Processus de Connexion WiFi

```
1. Scan/Discovery
   Client → Broadcast Probe Request
   AP → Probe Response (SSID, Capabilities)

2. Authentication
   Client → Authentication Request
   AP → Authentication Response

3. Association
   Client → Association Request
   AP → Association Response

4. 4-Way Handshake (WPA/WPA2)
   Échange de clés pour chiffrement
```

---

## 2. Protocoles de Sécurité WiFi

### 2.1 WEP (Wired Equivalent Privacy) - OBSOLÈTE

**Année :** 1997 (Partie de 802.11 original)

**Fonctionnement :**
```
Chiffrement : RC4 (Stream Cipher)
Clé : 64 bits (40 bits effectifs) ou 128 bits (104 bits effectifs)
IV (Initialization Vector) : 24 bits
```

**Mécanisme :**
```c#
// Pseudo-code du chiffrement WEP
byte[] IV = GenerateRandomIV(24); // 24 bits
byte[] Key = UserKey; // 40 ou 104 bits
byte[] EncryptionKey = Concat(IV, Key);

byte[] Ciphertext = RC4(PlainText, EncryptionKey);
byte[] Packet = Concat(IV, Ciphertext, CRC32(PlainText));
```

**Vulnérabilités CRITIQUES :**

1. **IV trop court (24 bits)**
   - Seulement 16,777,216 combinaisons possibles
   - Réutilisation d'IV garantie après ~5,000 paquets
   - Attaque par collision d'IV

2. **CRC-32 non cryptographique**
   - Permet injection et modification de paquets
   - Pas d'authentification d'intégrité

3. **Clé statique**
   - Même clé pour tous les paquets
   - Analyse statistique possible

**Attaque pratique :**
```bash
# Aircrack-ng peut casser WEP en 5-10 minutes
airodump-ng wlan0mon
aireplay-ng --arpreplay -b [BSSID] wlan0mon
aircrack-ng capture.cap
```

**Pourquoi c'est cassable en quelques minutes :**
- Capture de 40,000-85,000 paquets suffit
- Analyse statistique des IV
- Récupération de la clé par force brute optimisée

---

### 2.2 WPA (Wi-Fi Protected Access)

**Année :** 2003 (Solution temporaire en attendant WPA2)

**Améliorations sur WEP :**

1. **TKIP (Temporal Key Integrity Protocol)**
   - Clé dynamique par paquet (pas statique comme WEP)
   - IV étendu à 48 bits (au lieu de 24)
   - MIC (Message Integrity Check) au lieu de CRC-32

2. **Mécanisme TKIP :**
```c#
// Génération de clé par paquet (TKIP)
byte[] TemporalKey = DeriveFromMasterKey();
byte[] PacketKey = MixingFunction(TemporalKey, MacAddress, SequenceNumber);
byte[] Ciphertext = RC4(PlainText, PacketKey);
```

**Authentification :**
- **WPA-Personal (PSK)** : Mot de passe partagé
- **WPA-Enterprise (802.1X)** : Serveur RADIUS

**Processus PSK :**
```
1. Passphrase utilisateur → PMK (Pairwise Master Key)
   PMK = PBKDF2(HMAC-SHA1, passphrase, SSID, 4096 iterations, 256 bits)

2. 4-Way Handshake
   - Échange de nonces (nombres aléatoires)
   - Dérivation de PTK (Pairwise Transient Key)
   - Génération de GTK (Group Temporal Key)
```

**Vulnérabilités :**

1. **TKIP reste basé sur RC4**
   - Attaques contre RC4 possibles
   - Chopchop attack

2. **Attaque par dictionnaire sur PSK**
   - Si mot de passe faible
   - Capture du handshake suffit
   - Attaque offline possible

---

### 2.3 WPA2 (802.11i) - Standard Actuel

**Année :** 2004

**Amélioration majeure : CCMP (Counter Mode CBC-MAC Protocol)**

**Chiffrement :**
```
Algorithme : AES (Advanced Encryption Standard)
Mode : Counter Mode + CBC-MAC
Clé : 128 bits (256 bits en Enterprise)
```

**Fonctionnement CCMP :**
```c#
// Pseudo-code CCMP
public class CCMP
{
    private AesCryptoServiceProvider aes;

    public byte[] Encrypt(byte[] plaintext, byte[] key, byte[] nonce)
    {
        // Counter Mode pour chiffrement
        byte[] ciphertext = AES_CTR_Encrypt(plaintext, key, nonce);

        // CBC-MAC pour authentification
        byte[] mac = AES_CBC_MAC(ciphertext, key);

        return Concat(ciphertext, mac);
    }
}
```

**4-Way Handshake (WPA2-PSK) :**
```
Message 1: AP → Client
   ANonce (AP Nonce - nombre aléatoire)

Message 2: Client → AP
   SNonce (Station Nonce)
   MIC (Message Integrity Check)
   PTK dérivé = f(PMK, ANonce, SNonce, MAC_AP, MAC_Client)

Message 3: AP → Client
   GTK chiffré avec KEK (Key Encryption Key)
   MIC

Message 4: Client → AP
   Confirmation
   MIC
```

**Dérivation des clés :**
```c#
// Pairwise Master Key (depuis passphrase)
PMK = PBKDF2_SHA1(
    password: passphrase,
    salt: SSID,
    iterations: 4096,
    keyLength: 256
);

// Pairwise Transient Key (depuis PMK)
PTK = PRF-512(
    PMK,
    "Pairwise key expansion",
    Min(MAC_AP, MAC_Client) + Max(MAC_AP, MAC_Client) +
    Min(ANonce, SNonce) + Max(ANonce, SNonce)
);

// PTK contient :
// - KCK (128 bits) : Key Confirmation Key (pour MIC)
// - KEK (128 bits) : Key Encryption Key (pour GTK)
// - TK (128 bits)  : Temporal Key (pour données)
```

**Vulnérabilités WPA2 :**

1. **Attaque par dictionnaire sur PSK**
```python
# Si mot de passe faible
# Capture du 4-way handshake
# Test de millions de mots de passe
for password in dictionary:
    PMK = PBKDF2(password, SSID, 4096)
    PTK = PRF(PMK, ...)
    if verify_MIC(handshake, PTK):
        print("Password found:", password)
```

2. **KRACK (Key Reinstallation Attack) - 2017**
```
Principe :
- Rejouer Message 3 du handshake
- Forcer réinstallation de la même clé
- Réinitialisation du nonce
- Permet déchiffrement de paquets
```

3. **Attaque PMKID (sans handshake complet)**
```
PMKID = HMAC-SHA1-128(PMK, "PMK Name" | MAC_AP | MAC_STA)

Récupération :
- Envoi d'une association request
- AP répond avec PMKID dans EAPOL frame
- Attaque offline sur PMKID (plus rapide que handshake)
```

---

### 2.4 WPA3 (802.11-2020) - Nouvelle Génération

**Année :** 2018

**Innovation majeure : SAE (Simultaneous Authentication of Equals)**

**SAE remplace PSK :**
```
Basé sur : Dragonfly Key Exchange (résistant aux attaques par dictionnaire)

Principe :
- Pas de dérivation directe depuis password
- Échange cryptographique résistant aux attaques offline
- Forward secrecy
```

**Processus SAE :**
```c#
// Dragonfly Handshake
public class SAE
{
    // 1. Commit Exchange
    public (BigInteger scalar, ECPoint element) Commit(string password)
    {
        BigInteger pwd_seed = H(password, MAC_A, MAC_B);
        ECPoint pwd_value = DerivePasswordElement(pwd_seed);

        BigInteger rand = GenerateRandom();
        BigInteger mask = GenerateRandom();

        BigInteger scalar = (rand + mask) mod q;
        ECPoint element = (mask * pwd_value) + (rand * G);

        return (scalar, element);
    }

    // 2. Confirm Exchange
    public byte[] Confirm(BigInteger shared_secret)
    {
        byte[] kck = KDF(shared_secret, "confirm key");
        return HMAC(kck, scalar_A + scalar_B + element_A + element_B);
    }
}
```

**Avantages WPA3 :**

1. **Protection contre attaques par dictionnaire offline**
   - Impossible de capturer et tester des mots de passe
   - Chaque tentative nécessite interaction avec l'AP

2. **Forward Secrecy**
   - Compromission du password n'affecte pas sessions passées
   - Nouvelles clés pour chaque session

3. **Chiffrement renforcé**
   - 192-bit en mode Enterprise (WPA3-Enterprise)
   - Suite cryptographique : GCMP-256, HMAC-SHA-384, ECDHE-384

4. **Protection contre downgrade attacks**
   - Transition Management (empêche forcer WPA2)

**WPA3 Modes :**
```
WPA3-Personal (SAE)
- Remplace WPA2-PSK
- Meilleur pour maison/PME

WPA3-Enterprise (192-bit)
- Suite cryptographique renforcée
- Pour organisations avec besoins sécurité élevés

WPA3-Transition
- Support WPA2 et WPA3 simultanément
- Pour migration progressive
```

---

### 2.5 WPS (Wi-Fi Protected Setup) - VULNÉRABILITÉ MAJEURE

**But :** Simplifier la connexion WiFi

**Méthodes :**
1. **PIN** : Code à 8 chiffres
2. **Push Button** : Appuyer sur bouton physique
3. **NFC** : Communication en champ proche

**Vulnérabilité CRITIQUE du PIN :**

```
PIN = 8 chiffres, mais :
- Dernier chiffre est un checksum
- PIN = [4 premiers] + [3 suivants] + [checksum]

Espace de recherche :
- Premier groupe : 10,000 possibilités (0000-9999)
- Second groupe : 1,000 possibilités (000-999)
- Total : 11,000 essais maximum (au lieu de 100,000,000)
```

**Attaque Reaver (Force Brute) :**
```bash
# 4-8 heures pour tester tous les PIN
reaver -i wlan0mon -b [BSSID] -vv
```

**Attaque Pixie Dust (Faille implémentation) :**
```
Certains routeurs génèrent PIN avec :
- Entropie insuffisante
- Utilisation de seed prévisible
- Récupération du PIN en quelques secondes

Principe :
- Analyser les nonces dans M1-M3
- Si générés avec PRNG faible
- Récupération du seed
- Calcul du PIN instantané
```

**Code conceptuel :**
```c#
public class WPSPinAttack
{
    public bool TryPin(string bssid, int firstHalf, int secondHalf)
    {
        // Le PIN WPS est divisé en deux parties
        // Vérifié séparément par l'AP

        // Test première moitié (10,000 possibilités)
        if (TestFirstHalf(bssid, firstHalf))
        {
            // Test seconde moitié (1,000 possibilités)
            if (TestSecondHalf(bssid, secondHalf))
            {
                string pin = $"{firstHalf:D4}{secondHalf:D3}";
                return true;
            }
        }
        return false;
    }

    public void BruteForce(string bssid)
    {
        // Maximum 11,000 tentatives
        for (int first = 0; first < 10000; first++)
        {
            if (TestFirstHalf(bssid, first))
            {
                for (int second = 0; second < 1000; second++)
                {
                    if (TryPin(bssid, first, second))
                    {
                        Console.WriteLine($"PIN trouvé: {first:D4}{second:D3}");
                        return;
                    }
                }
            }
        }
    }
}
```

**Protection :**
- **DÉSACTIVER WPS** (recommandation #1)
- Certains AP limitent les tentatives (rate limiting)
- WPS 2.0 a des protections améliorées

---

## 3. Attaques WiFi (Théorie - À Connaître)

### 3.1 Attaque par Dictionnaire (WPA/WPA2-PSK)

**Prérequis :**
- Capture du 4-way handshake
- Dictionnaire de mots de passe

**Processus :**
```c#
public class WPA2DictionaryAttack
{
    public string CrackPassword(
        byte[] handshake,
        string ssid,
        List<string> dictionary)
    {
        // Extraction du handshake
        var (mac_ap, mac_sta, anonce, snonce, mic) =
            ParseHandshake(handshake);

        foreach (string password in dictionary)
        {
            // 1. Calculer PMK (lent - 4096 itérations)
            byte[] pmk = PBKDF2_SHA1(
                password: Encoding.UTF8.GetBytes(password),
                salt: Encoding.UTF8.GetBytes(ssid),
                iterations: 4096,
                keyLength: 32
            );

            // 2. Calculer PTK
            byte[] ptk = PRF512(
                pmk,
                "Pairwise key expansion",
                Min(mac_ap, mac_sta) + Max(mac_ap, mac_sta) +
                Min(anonce, snonce) + Max(anonce, snonce)
            );

            // 3. Extraire KCK (premiers 16 octets de PTK)
            byte[] kck = ptk.Take(16).ToArray();

            // 4. Calculer MIC attendu
            byte[] calculatedMic = HMAC_SHA1(kck, handshakeData);

            // 5. Comparer avec MIC capturé
            if (calculatedMic.SequenceEqual(mic))
            {
                return password; // Trouvé !
            }
        }

        return null; // Pas trouvé
    }
}
```

**Optimisations :**
```c#
// Pré-calcul des PMK (Rainbow Tables)
public class RainbowTable
{
    // Pour SSID communs (linksys, netgear, etc.)
    Dictionary<string, Dictionary<string, byte[]>> tables;

    public void PrecomputePMK(string ssid, List<string> passwords)
    {
        tables[ssid] = new Dictionary<string, byte[]>();

        Parallel.ForEach(passwords, password =>
        {
            byte[] pmk = PBKDF2_SHA1(password, ssid, 4096, 32);
            lock(tables[ssid])
            {
                tables[ssid][password] = pmk;
            }
        });
    }

    // Gain : 4096 itérations → 1 lookup
}
```

**Temps estimé :**
- 1 password test ≈ 0.1-1ms (selon CPU)
- Dictionnaire 1 million de mots ≈ 100-1000 secondes
- Avec GPU : 100,000 tests/seconde

**Contre-mesures :**
- Mot de passe long (12+ caractères)
- Aléatoire (pas de mot du dictionnaire)
- Mélange caractères (a-Z, 0-9, symboles)

---

### 3.2 Evil Twin Attack

**Principe :** Créer un faux point d'accès identique au légitime

**Scénario :**
```
1. Attaquant crée un AP avec même SSID
2. Signal plus fort que l'AP légitime
3. Client se connecte au faux AP
4. Attaquant intercepte tout le trafic (MITM)
```

**Implémentation conceptuelle :**
```c#
public class EvilTwinAP
{
    public void CreateFakeAP(string targetSSID, string targetBSSID)
    {
        // 1. Configurer interface en mode AP
        ConfigureWirelessInterface(
            ssid: targetSSID,
            // Utiliser un BSSID similaire ou identique
            bssid: GenerateSimilarBSSID(targetBSSID),
            channel: DetectTargetChannel(targetBSSID),
            // Signal plus fort
            txPower: "30dBm"
        );

        // 2. Serveur DHCP
        StartDHCPServer("192.168.1.0/24");

        // 3. DNS Spoofing
        StartDNSServer(redirectTo: "192.168.1.1");

        // 4. Capture du trafic
        StartPacketCapture();

        // 5. (Optionnel) Deauth attack sur AP légitime
        SendDeauthFrames(targetBSSID);
    }

    public void CaptureCredentials()
    {
        // Portal captif : demande "re-connexion"
        StartCaptivePortal(cloneOf: "original-login-page.com");

        // Capture identifiants quand utilisateur se "reconnecte"
    }
}
```

**Détection (côté victime) :**
```c#
public bool DetectEvilTwin(string expectedSSID)
{
    var networks = ScanNetworks();

    // Multiple AP avec même SSID
    var duplicates = networks
        .Where(n => n.SSID == expectedSSID)
        .ToList();

    if (duplicates.Count > 1)
    {
        // Comparer BSSID, channel, encryption
        foreach (var net in duplicates)
        {
            if (net.SecurityType != "WPA2" ||
                net.Channel != expectedChannel)
            {
                Alert("Possible Evil Twin détecté!");
                return true;
            }
        }
    }

    return false;
}
```

**Protection :**
- Vérifier le certificat (WPA2-Enterprise)
- Utiliser VPN
- Vérifier BSSID du réseau connu
- Désactiver auto-connexion aux réseaux ouverts

---

### 3.3 Deauthentication Attack

**Principe :** Forcer la déconnexion d'un client

**Fonctionnement :**
```
1. Attaquant envoie des frames de deauthentication
2. Frames non chiffrées dans 802.11 (avant 802.11w)
3. Client pense que l'AP le déconnecte
4. Client se déconnecte
```

**Code conceptuel :**
```c#
public class DeauthAttack
{
    public void DeauthenticateClient(
        string apBSSID,
        string clientMAC,
        int packetCount = 10)
    {
        // Frame de deauthentication 802.11
        var frame = new Dot11DeauthFrame
        {
            // Header
            Type = 0,       // Management frame
            Subtype = 12,   // Deauthentication

            // Addresses
            DestinationAddress = clientMAC,
            SourceAddress = apBSSID,
            BSSID = apBSSID,

            // Reason
            ReasonCode = 7  // Class 3 frame from nonassociated STA
        };

        // Envoyer multiple fois
        for (int i = 0; i < packetCount; i++)
        {
            SendRawFrame(frame);
            Thread.Sleep(100);
        }
    }

    public void DeauthAllClients(string apBSSID)
    {
        // Broadcast deauth (disconnect tous les clients)
        DeauthenticateClient(
            apBSSID,
            clientMAC: "FF:FF:FF:FF:FF:FF", // Broadcast
            packetCount: 50
        );
    }
}
```

**Utilisations (malveillantes) :**
1. Forcer reconnexion pour capturer handshake
2. Déni de service (DoS)
3. Forcer connexion à Evil Twin

**Protection : 802.11w (PMF - Protected Management Frames)**
```c#
// WPA3 et WPA2 avec 802.11w
public class PMF
{
    public byte[] ProtectManagementFrame(byte[] frame, byte[] key)
    {
        // Chiffrement des management frames
        byte[] protectedFrame = AES_CMAC(frame, key);
        return protectedFrame;
    }

    // Les frames deauth/disassoc sont maintenant chiffrées
    // Impossible de spoof sans connaître la clé
}
```

---

## 4. Canaux et Interférences

### 4.1 Canaux WiFi

**Bande 2.4 GHz :**
```
Canal 1  : 2412 MHz
Canal 2  : 2417 MHz
Canal 3  : 2422 MHz
...
Canal 11 : 2462 MHz (US)
Canal 13 : 2472 MHz (Europe)
Canal 14 : 2484 MHz (Japon uniquement)

Largeur canal : 20 MHz (ou 22 MHz incluant bandes de garde)
```

**Chevauchement des canaux :**
```
Canal 1 : 2401-2423 MHz
Canal 2 : 2406-2428 MHz  ← Chevauche canal 1
Canal 3 : 2411-2433 MHz  ← Chevauche canaux 1 et 2

Canaux non-chevauchants (US) : 1, 6, 11
```

**Calcul du canal depuis fréquence :**
```c#
public class WiFiChannelCalculator
{
    public int GetChannel24GHz(int frequencyMHz)
    {
        if (frequencyMHz == 2484)
            return 14; // Canal spécial Japon

        if (frequencyMHz >= 2412 && frequencyMHz <= 2472)
        {
            return (frequencyMHz - 2412) / 5 + 1;
        }

        return 0; // Invalid
    }

    public int GetChannel5GHz(int frequencyMHz)
    {
        if (frequencyMHz >= 5170 && frequencyMHz <= 5825)
        {
            return (frequencyMHz - 5170) / 5 + 34;
        }

        return 0; // Invalid
    }

    public int GetFrequency(int channel)
    {
        if (channel >= 1 && channel <= 13)
        {
            return 2412 + (channel - 1) * 5;
        }
        else if (channel == 14)
        {
            return 2484;
        }
        else if (channel >= 34 && channel <= 196)
        {
            return 5170 + (channel - 34) * 5;
        }

        return 0; // Invalid
    }
}
```

**Bande 5 GHz :**
```
Canaux : 36, 40, 44, 48, 52, 56, 60, 64, 100-144, 149-165
Largeur : 20, 40, 80, 160 MHz (selon 802.11ac/ax)

Avantages :
- Moins de congestion
- Plus de canaux non-chevauchants
- Débits plus élevés

Inconvénients :
- Portée plus courte
- Atténuation par obstacles plus forte
```

---

## 5. Mesure de Signal

### 5.1 RSSI et dBm

**RSSI (Received Signal Strength Indicator) :**
```
Mesure : dBm (decibels milliwatt)
Échelle : -30 dBm (excellent) à -90 dBm (très faible)

Référence :
0 dBm = 1 mW
-30 dBm = Excellent (très proche)
-50 dBm = Très bon
-60 dBm = Bon
-70 dBm = Moyen
-80 dBm = Faible
-90 dBm = Très faible (limite connexion)
```

**Conversion et calcul :**
```c#
public class SignalStrengthCalculator
{
    // Convertir puissance (mW) en dBm
    public double MilliwattsToDbm(double milliwatts)
    {
        return 10 * Math.Log10(milliwatts);
    }

    // Convertir dBm en mW
    public double DbmToMilliwatts(double dbm)
    {
        return Math.Pow(10, dbm / 10);
    }

    // Qualité du signal en pourcentage
    public int GetSignalQualityPercent(int rssiDbm)
    {
        // Méthode 1: Linéaire
        if (rssiDbm <= -100)
            return 0;
        else if (rssiDbm >= -50)
            return 100;
        else
            return 2 * (rssiDbm + 100);
    }

    // Estimation distance (approximative)
    public double EstimateDistance(int rssiDbm, int txPowerDbm = 20)
    {
        // FSPL (Free Space Path Loss) à 2.4 GHz
        double frequency = 2400; // MHz
        double pathLoss = txPowerDbm - rssiDbm;

        // FSPL = 20*log10(d) + 20*log10(f) + 32.44
        // d = 10^((FSPL - 20*log10(f) - 32.44) / 20)

        double exponent = (pathLoss - 20 * Math.Log10(frequency) - 32.44) / 20;
        double distanceKm = Math.Pow(10, exponent);

        return distanceKm * 1000; // Convertir en mètres
    }

    // Qualité descriptive
    public string GetSignalQuality(int rssiDbm)
    {
        return rssiDbm switch
        {
            >= -50 => "Excellent",
            >= -60 => "Très bon",
            >= -67 => "Bon",
            >= -70 => "Moyen",
            >= -80 => "Faible",
            _ => "Très faible"
        };
    }
}
```

**SNR (Signal-to-Noise Ratio) :**
```c#
public class SNRCalculator
{
    public int CalculateSNR(int signalDbm, int noiseDbm)
    {
        // SNR = Signal - Noise (en dB)
        return signalDbm - noiseDbm;
    }

    public string GetConnectionQuality(int snrDb)
    {
        return snrDb switch
        {
            > 40 => "Excellent",
            > 25 => "Très bon",
            > 15 => "Bon",
            > 10 => "Moyen",
            > 5 => "Faible",
            _ => "Très faible"
        };
    }
}
```

---

# PARTIE II - C# ET .NET

## 1. Fondamentaux C#

### 1.1 Types et Variables

**Types valeur vs référence :**
```c#
// TYPES VALEUR (stockés sur la pile - stack)
int age = 25;              // System.Int32
double price = 19.99;      // System.Double
bool isActive = true;      // System.Boolean
char letter = 'A';         // System.Char
DateTime date = DateTime.Now; // struct

// Structs personnalisés
public struct Point
{
    public int X;
    public int Y;
}

// TYPES RÉFÉRENCE (stockés sur le tas - heap)
string name = "WiFi";      // System.String
object obj = new object(); // System.Object
int[] numbers = {1, 2, 3}; // Array

// Classes personnalisées
public class WifiNetwork
{
    public string Ssid { get; set; }
    public int Signal { get; set; }
}
```

**Différence importante :**
```c#
// Types valeur : copie de valeur
int a = 10;
int b = a;
b = 20;
Console.WriteLine(a); // 10 (inchangé)

// Types référence : copie de référence
var network1 = new WifiNetwork { Ssid = "Test" };
var network2 = network1;
network2.Ssid = "Modified";
Console.WriteLine(network1.Ssid); // "Modified" (modifié !)
```

### 1.2 Properties (Propriétés)

**Auto-implemented properties :**
```c#
public class WifiNetwork
{
    // Propriété auto-implémentée
    public string Ssid { get; set; }

    // Avec valeur par défaut (C# 6+)
    public int SignalStrength { get; set; } = -100;

    // Read-only (init-only en C# 9+)
    public string Bssid { get; init; }

    // Calculée (computed property)
    public string SignalQuality => GetSignalQuality();

    // Avec logique personnalisée
    private int _channel;
    public int Channel
    {
        get => _channel;
        set
        {
            if (value < 1 || value > 14)
                throw new ArgumentException("Canal invalide");
            _channel = value;
        }
    }

    private string GetSignalQuality()
    {
        return SignalStrength switch
        {
            >= -50 => "Excellent",
            >= -60 => "Bon",
            >= -70 => "Moyen",
            _ => "Faible"
        };
    }
}
```

**Expression-bodied members (C# 6+) :**
```c#
public class SecurityAnalysis
{
    // Property expression-bodied
    public int Score { get; set; }
    public string RiskLevel => Score >= 8 ? "Faible" : "Élevé";

    // Method expression-bodied
    public bool IsSecure() => Score >= 7;

    // Constructor expression-bodied (C# 7+)
    public SecurityAnalysis(int score) => Score = score;
}
```

### 1.3 Records (C# 9+)

**Immutabilité et égalité par valeur :**
```c#
// Record traditionnel
public record WifiNetwork(string Ssid, string Bssid, int SignalStrength);

// Utilisation
var network1 = new WifiNetwork("MyWiFi", "00:11:22:33:44:55", -60);
var network2 = new WifiNetwork("MyWiFi", "00:11:22:33:44:55", -60);

Console.WriteLine(network1 == network2); // true (égalité par valeur)

// Modification avec "with"
var network3 = network1 with { SignalStrength = -50 };

// Record avec propriétés mutables
public record WifiNetworkMutable
{
    public string Ssid { get; init; }
    public string Bssid { get; init; }
    public int SignalStrength { get; set; } // Mutable
}
```

**Record vs Class :**
```c#
// CLASS (référence, égalité par référence)
public class NetworkClass
{
    public string Ssid { get; set; }
}

var c1 = new NetworkClass { Ssid = "Test" };
var c2 = new NetworkClass { Ssid = "Test" };
Console.WriteLine(c1 == c2); // false (références différentes)

// RECORD (référence, mais égalité par valeur)
public record NetworkRecord(string Ssid);

var r1 = new NetworkRecord("Test");
var r2 = new NetworkRecord("Test");
Console.WriteLine(r1 == r2); // true (même valeur)
```

### 1.4 Async/Await

**Programmation asynchrone :**
```c#
public class WifiScanner
{
    // Méthode synchrone (bloque le thread)
    public List<WifiNetwork> ScanNetworksSync()
    {
        Thread.Sleep(2000); // Bloque pendant 2 secondes
        return new List<WifiNetwork>();
    }

    // Méthode asynchrone (non-bloquante)
    public async Task<List<WifiNetwork>> ScanNetworksAsync()
    {
        // Simule opération longue sans bloquer
        await Task.Delay(2000);
        return new List<WifiNetwork>();
    }

    // Avec CancellationToken
    public async Task<List<WifiNetwork>> ScanNetworksAsync(
        CancellationToken cancellationToken)
    {
        for (int i = 0; i < 10; i++)
        {
            // Vérifier si annulation demandée
            cancellationToken.ThrowIfCancellationRequested();

            await Task.Delay(200, cancellationToken);
        }

        return new List<WifiNetwork>();
    }
}

// Utilisation
public async Task Example()
{
    var scanner = new WifiScanner();

    // Appel asynchrone
    var networks = await scanner.ScanNetworksAsync();

    // Avec timeout
    var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
    try
    {
        networks = await scanner.ScanNetworksAsync(cts.Token);
    }
    catch (OperationCanceledException)
    {
        Console.WriteLine("Scan annulé ou timeout");
    }
}
```

**ConfigureAwait :**
```c#
public async Task<string> GetDataAsync()
{
    // Dans une bibliothèque : éviter capture du contexte
    var result = await HttpClient.GetStringAsync(url)
        .ConfigureAwait(false);

    // Dans UI : capturer le contexte (par défaut)
    var networks = await ScanNetworksAsync();
    NetworkList.ItemsSource = networks; // Retour sur UI thread
}
```

**Task.WhenAll et Task.WhenAny :**
```c#
public async Task ParallelScansExample()
{
    var tasks = new List<Task<List<WifiNetwork>>>
    {
        ScanChannel(1),
        ScanChannel(6),
        ScanChannel(11)
    };

    // Attendre toutes les tâches
    var results = await Task.WhenAll(tasks);
    var allNetworks = results.SelectMany(r => r).ToList();

    // Ou attendre la première qui termine
    var firstResult = await Task.WhenAny(tasks);
    var firstNetworks = await firstResult;
}
```

### 1.5 LINQ (Language Integrated Query)

**Opérations sur collections :**
```c#
public class LinqExamples
{
    public void DemoLinq(List<WifiNetwork> networks)
    {
        // Where - Filtrer
        var secureNetworks = networks
            .Where(n => n.SecurityType.Contains("WPA2"))
            .ToList();

        // Select - Projeter
        var ssids = networks
            .Select(n => n.Ssid)
            .ToList();

        // OrderBy - Trier
        var sortedBySignal = networks
            .OrderByDescending(n => n.SignalStrength)
            .ToList();

        // First / FirstOrDefault
        var strongest = networks
            .OrderByDescending(n => n.SignalStrength)
            .FirstOrDefault();

        // Any / All
        bool hasWep = networks.Any(n => n.SecurityType.Contains("WEP"));
        bool allSecure = networks.All(n => n.SecurityScore >= 7);

        // Count
        int wpa3Count = networks.Count(n => n.SecurityType.Contains("WPA3"));

        // GroupBy
        var bySecurityType = networks
            .GroupBy(n => n.SecurityType)
            .Select(g => new
            {
                SecurityType = g.Key,
                Count = g.Count(),
                Networks = g.ToList()
            });

        // Average / Sum / Min / Max
        double avgSignal = networks.Average(n => n.SignalStrength);
        int minSignal = networks.Min(n => n.SignalStrength);
        int maxScore = networks.Max(n => n.SecurityScore);

        // Distinct
        var uniqueSecurityTypes = networks
            .Select(n => n.SecurityType)
            .Distinct()
            .ToList();

        // Take / Skip (pagination)
        var first10 = networks.Take(10).ToList();
        var next10 = networks.Skip(10).Take(10).ToList();

        // Join
        var savedNetworks = GetSavedNetworks();
        var joinedData = networks
            .Join(savedNetworks,
                n => n.Ssid,
                s => s.Ssid,
                (n, s) => new { Network = n, Saved = s })
            .ToList();
    }

    // Query syntax (alternative)
    public void QuerySyntaxExample(List<WifiNetwork> networks)
    {
        var query = from n in networks
                    where n.SecurityScore >= 7
                    orderby n.SignalStrength descending
                    select new
                    {
                        n.Ssid,
                        n.SecurityType,
                        Quality = n.SignalQuality
                    };

        var results = query.ToList();
    }
}
```

### 1.6 Pattern Matching (C# 8+)

**Switch expressions :**
```c#
public class PatternMatchingExamples
{
    // Switch expression
    public string GetSecurityLevel(string securityType) => securityType switch
    {
        "WPA3" => "Très élevé",
        "WPA2" => "Élevé",
        "WPA" => "Moyen",
        "WEP" => "Faible",
        _ => "Inconnu"
    };

    // Pattern matching avec when
    public string AnalyzeNetwork(WifiNetwork network) => network switch
    {
        { SecurityType: "WEP" } => "DANGER : WEP obsolète",
        { SecurityType: "WPA2", IsWpsEnabled: true } => "ATTENTION : WPS activé",
        { SecurityScore: >= 8 } => "Sécurisé",
        { SecurityScore: >= 5 } => "Moyennement sécurisé",
        _ => "Non sécurisé"
    };

    // Type patterns
    public void ProcessResult(object result)
    {
        switch (result)
        {
            case WifiNetwork network:
                Console.WriteLine($"Network: {network.Ssid}");
                break;
            case List<WifiNetwork> networks:
                Console.WriteLine($"Networks: {networks.Count}");
                break;
            case null:
                Console.WriteLine("Null result");
                break;
            default:
                Console.WriteLine("Unknown type");
                break;
        }
    }

    // Positional patterns (records/tuples)
    public string GetRiskLevel(int score, bool wpsEnabled) =>
        (score, wpsEnabled) switch
        {
            (>= 8, false) => "Faible",
            (>= 5, false) => "Moyen",
            (_, true) => "Élevé (WPS)",
            _ => "Critique"
        };
}
```

---

## 2. Collections et Génériques

### 2.1 Types de Collections

```c#
public class CollectionsDemo
{
    // List<T> - Liste dynamique (la plus utilisée)
    public void ListExample()
    {
        var networks = new List<WifiNetwork>();

        // Ajouter
        networks.Add(new WifiNetwork { Ssid = "Test" });
        networks.AddRange(GetMoreNetworks());

        // Accéder
        var first = networks[0];
        var last = networks[networks.Count - 1];

        // Chercher
        var found = networks.Find(n => n.Ssid == "Test");
        var index = networks.FindIndex(n => n.SecurityScore >= 8);

        // Supprimer
        networks.Remove(first);
        networks.RemoveAt(0);
        networks.RemoveAll(n => n.SecurityScore < 5);

        // Parcourir
        foreach (var network in networks)
        {
            Console.WriteLine(network.Ssid);
        }
    }

    // Dictionary<TKey, TValue> - Table de hachage
    public void DictionaryExample()
    {
        var networksByBssid = new Dictionary<string, WifiNetwork>();

        // Ajouter
        networksByBssid["00:11:22:33:44:55"] = new WifiNetwork
        {
            Ssid = "Test"
        };
        networksByBssid.Add("AA:BB:CC:DD:EE:FF", new WifiNetwork
        {
            Ssid = "Test2"
        });

        // Vérifier existence
        if (networksByBssid.ContainsKey("00:11:22:33:44:55"))
        {
            var network = networksByBssid["00:11:22:33:44:55"];
        }

        // TryGetValue (plus sûr)
        if (networksByBssid.TryGetValue("00:11:22:33:44:55", out var found))
        {
            Console.WriteLine(found.Ssid);
        }

        // Parcourir
        foreach (var kvp in networksByBssid)
        {
            Console.WriteLine($"{kvp.Key}: {kvp.Value.Ssid}");
        }
    }

    // HashSet<T> - Ensemble unique
    public void HashSetExample()
    {
        var uniqueSSIDs = new HashSet<string>();

        // Ajouter (ignore duplicata)
        uniqueSSIDs.Add("WiFi1");
        uniqueSSIDs.Add("WiFi2");
        uniqueSSIDs.Add("WiFi1"); // Ignoré

        Console.WriteLine(uniqueSSIDs.Count); // 2

        // Opérations d'ensemble
        var set1 = new HashSet<string> { "A", "B", "C" };
        var set2 = new HashSet<string> { "B", "C", "D" };

        set1.UnionWith(set2);        // A, B, C, D
        set1.IntersectWith(set2);    // B, C
        set1.ExceptWith(set2);       // A
    }

    // Queue<T> - File FIFO
    public void QueueExample()
    {
        var scanQueue = new Queue<string>();

        scanQueue.Enqueue("Channel 1");
        scanQueue.Enqueue("Channel 6");
        scanQueue.Enqueue("Channel 11");

        while (scanQueue.Count > 0)
        {
            var channel = scanQueue.Dequeue();
            Console.WriteLine($"Scanning {channel}");
        }
    }

    // Stack<T> - Pile LIFO
    public void StackExample()
    {
        var history = new Stack<string>();

        history.Push("Page1");
        history.Push("Page2");
        history.Push("Page3");

        var current = history.Pop(); // "Page3"
        var previous = history.Peek(); // "Page2" (sans retirer)
    }

    // ObservableCollection<T> - Pour data binding (MAUI/WPF)
    public void ObservableCollectionExample()
    {
        var networks = new ObservableCollection<WifiNetwork>();

        // Événement quand collection modifiée
        networks.CollectionChanged += (sender, e) =>
        {
            Console.WriteLine($"Action: {e.Action}");
        };

        networks.Add(new WifiNetwork()); // Déclenche événement
    }
}
```

### 2.2 Génériques

```c#
// Classe générique
public class Repository<T> where T : class
{
    private List<T> items = new List<T>();

    public void Add(T item) => items.Add(item);

    public T Get(int index) => items[index];

    public List<T> GetAll() => items;

    public T Find(Predicate<T> predicate) => items.Find(predicate);
}

// Utilisation
var networkRepo = new Repository<WifiNetwork>();
networkRepo.Add(new WifiNetwork { Ssid = "Test" });

// Méthode générique
public class GenericMethods
{
    public T Max<T>(T a, T b) where T : IComparable<T>
    {
        return a.CompareTo(b) > 0 ? a : b;
    }

    public List<TOutput> ConvertList<TInput, TOutput>(
        List<TInput> input,
        Func<TInput, TOutput> converter)
    {
        return input.Select(converter).ToList();
    }
}

// Contraintes génériques
public class Constraints
{
    // T doit être une classe
    public void Method1<T>(T item) where T : class { }

    // T doit être un struct
    public void Method2<T>(T item) where T : struct { }

    // T doit hériter de WifiNetwork
    public void Method3<T>(T item) where T : WifiNetwork { }

    // T doit implémenter IComparable
    public void Method4<T>(T item) where T : IComparable<T> { }

    // T doit avoir constructeur sans paramètre
    public void Method5<T>() where T : new()
    {
        T instance = new T();
    }

    // Multiples contraintes
    public void Method6<T>(T item)
        where T : class, IDisposable, new() { }
}
```

---

## 3. Delegates, Events et Lambda

### 3.1 Delegates

```c#
// Déclaration de delegate
public delegate void NetworkFoundHandler(WifiNetwork network);
public delegate bool NetworkFilter(WifiNetwork network);
public delegate int NetworkComparer(WifiNetwork a, WifiNetwork b);

public class DelegateExamples
{
    // Utilisation simple
    public void BasicDelegate()
    {
        NetworkFilter isSecure = network => network.SecurityScore >= 7;

        var network = new WifiNetwork { SecurityScore = 8 };
        if (isSecure(network))
        {
            Console.WriteLine("Network is secure");
        }
    }

    // Multicast delegate
    public void MulticastDelegate()
    {
        NetworkFoundHandler handler = null;

        // Ajouter plusieurs méthodes
        handler += OnNetworkFound1;
        handler += OnNetworkFound2;
        handler += OnNetworkFound3;

        // Appeler toutes les méthodes
        handler(new WifiNetwork { Ssid = "Test" });

        // Retirer une méthode
        handler -= OnNetworkFound2;
    }

    private void OnNetworkFound1(WifiNetwork network)
        => Console.WriteLine($"Handler 1: {network.Ssid}");
    private void OnNetworkFound2(WifiNetwork network)
        => Console.WriteLine($"Handler 2: {network.Ssid}");
    private void OnNetworkFound3(WifiNetwork network)
        => Console.WriteLine($"Handler 3: {network.Ssid}");

    // Func<> et Action<> (delegates prédéfinis)
    public void BuiltInDelegates()
    {
        // Action<T> - Pas de retour
        Action<string> log = message => Console.WriteLine(message);
        log("Hello");

        // Func<TInput, TOutput> - Avec retour
        Func<int, int, int> add = (a, b) => a + b;
        int result = add(5, 3);

        // Predicate<T> - Retourne bool
        Predicate<WifiNetwork> isWep = n => n.SecurityType.Contains("WEP");

        // Utilisation dans méthodes LINQ
        List<WifiNetwork> networks = GetNetworks();

        Func<WifiNetwork, bool> filter = n => n.SecurityScore >= 7;
        var secureNetworks = networks.Where(filter).ToList();

        Func<WifiNetwork, string> selector = n => n.Ssid;
        var ssids = networks.Select(selector).ToList();
    }
}
```

### 3.2 Events

```c#
public class WifiScanner
{
    // Déclaration d'événement
    public event EventHandler<NetworkFoundEventArgs> NetworkFound;
    public event EventHandler ScanCompleted;

    // Méthode pour déclencher l'événement
    protected virtual void OnNetworkFound(WifiNetwork network)
    {
        // Vérifier si quelqu'un écoute
        NetworkFound?.Invoke(this, new NetworkFoundEventArgs(network));
    }

    protected virtual void OnScanCompleted()
    {
        ScanCompleted?.Invoke(this, EventArgs.Empty);
    }

    public async Task ScanAsync()
    {
        for (int i = 0; i < 10; i++)
        {
            await Task.Delay(100);

            var network = new WifiNetwork
            {
                Ssid = $"Network{i}"
            };

            // Déclencher événement
            OnNetworkFound(network);
        }

        OnScanCompleted();
    }
}

// EventArgs personnalisé
public class NetworkFoundEventArgs : EventArgs
{
    public WifiNetwork Network { get; }

    public NetworkFoundEventArgs(WifiNetwork network)
    {
        Network = network;
    }
}

// Utilisation
public class ScannerUsage
{
    public async Task UseScanner()
    {
        var scanner = new WifiScanner();

        // S'abonner aux événements
        scanner.NetworkFound += OnNetworkFound;
        scanner.ScanCompleted += OnScanCompleted;

        // Lancer le scan
        await scanner.ScanAsync();

        // Se désabonner
        scanner.NetworkFound -= OnNetworkFound;
        scanner.ScanCompleted -= OnScanCompleted;
    }

    private void OnNetworkFound(object sender, NetworkFoundEventArgs e)
    {
        Console.WriteLine($"Found: {e.Network.Ssid}");
    }

    private void OnScanCompleted(object sender, EventArgs e)
    {
        Console.WriteLine("Scan completed");
    }
}
```

### 3.3 Lambda Expressions

```c#
public class LambdaExamples
{
    public void BasicLambdas()
    {
        // Sans paramètres
        Func<int> getRandom = () => new Random().Next();

        // Un paramètre (parenthèses optionnelles)
        Func<int, int> square = x => x * x;
        Func<int, int> cube = (x) => x * x * x;

        // Plusieurs paramètres
        Func<int, int, int> add = (x, y) => x + y;

        // Avec bloc de code
        Func<int, int, int> multiply = (x, y) =>
        {
            int result = x * y;
            Console.WriteLine($"Result: {result}");
            return result;
        };

        // Capture de variables (closure)
        int factor = 10;
        Func<int, int> multiplyByFactor = x => x * factor;
        Console.WriteLine(multiplyByFactor(5)); // 50
    }

    public void LinqWithLambdas()
    {
        var networks = GetNetworks();

        // Where
        var filtered = networks.Where(n => n.SecurityScore >= 7);

        // Select
        var ssids = networks.Select(n => n.Ssid);

        // OrderBy
        var sorted = networks.OrderBy(n => n.SignalStrength);

        // Select avec objet anonyme
        var projection = networks.Select(n => new
        {
            n.Ssid,
            n.SecurityType,
            IsSecure = n.SecurityScore >= 7
        });

        // Chaînage
        var result = networks
            .Where(n => n.SecurityScore >= 7)
            .OrderByDescending(n => n.SignalStrength)
            .Select(n => n.Ssid)
            .Take(5)
            .ToList();
    }

    private List<WifiNetwork> GetNetworks() => new();
}
```

---

## 4. Gestion Mémoire et IDisposable

### 4.1 Garbage Collector

```c#
public class MemoryManagement
{
    public void GarbageCollectionDemo()
    {
        // Allocation sur le tas (heap)
        var network = new WifiNetwork(); // Référence

        // Plus de références → Éligible pour GC
        network = null;

        // Forcer le GC (à éviter en production)
        GC.Collect();
        GC.WaitForPendingFinalizers();

        // Vérifier génération
        int generation = GC.GetGeneration(network);
        // Gen 0 : Objets récents
        // Gen 1 : Objets intermédiaires
        // Gen 2 : Objets anciens
    }
}
```

### 4.2 IDisposable Pattern

```c#
// Classe avec ressources non-managées
public class WifiReceiver : IDisposable
{
    private IntPtr nativeHandle; // Ressource non-managée
    private bool disposed = false;

    public WifiReceiver()
    {
        // Allouer ressources
        nativeHandle = AllocateNativeResource();
    }

    // Implémentation IDisposable
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    // Méthode protégée pour libération
    protected virtual void Dispose(bool disposing)
    {
        if (!disposed)
        {
            if (disposing)
            {
                // Libérer ressources managées
                // (objets IDisposable)
            }

            // Libérer ressources non-managées
            if (nativeHandle != IntPtr.Zero)
            {
                FreeNativeResource(nativeHandle);
                nativeHandle = IntPtr.Zero;
            }

            disposed = true;
        }
    }

    // Finalizer (à utiliser seulement si ressources non-managées)
    ~WifiReceiver()
    {
        Dispose(false);
    }

    // Méthodes helper
    private IntPtr AllocateNativeResource() => IntPtr.Zero;
    private void FreeNativeResource(IntPtr handle) { }
}

// Utilisation avec using
public class Usage
{
    public void UseReceiver()
    {
        // using assure Dispose() appelé
        using (var receiver = new WifiReceiver())
        {
            // Utiliser receiver
        } // Dispose() appelé automatiquement ici

        // using declaration (C# 8+)
        using var receiver2 = new WifiReceiver();
        // Dispose() appelé à la fin du scope
    }
}
```

---

# PARTIE III - .NET MAUI

## 1. Architecture MAUI

### 1.1 Qu'est-ce que .NET MAUI ?

**.NET Multi-platform App UI**

**Évolution de Xamarin.Forms :**
```
Xamarin.Forms (2014-2021)
    ↓
.NET MAUI (2022+)
```

**Plateformes supportées :**
- Android (API 21+)
- iOS (14.2+)
- macOS (via Mac Catalyst)
- Windows (via WinUI 3)
- (Tizen)

**Architecture :**
```
┌─────────────────────────────────────┐
│      Application (.NET MAUI)        │
│  ┌─────────────────────────────┐   │
│  │  Shared Code (C#/XAML)      │   │
│  │  - Views                     │   │
│  │  - ViewModels               │   │
│  │  - Models                    │   │
│  │  - Services                  │   │
│  └─────────────────────────────┘   │
│              ↓                       │
│  ┌─────────────────────────────┐   │
│  │   Platform Abstractions     │   │
│  │   (Handlers/Renderers)      │   │
│  └─────────────────────────────┘   │
│         ↙  ↓  ↓  ↘                  │
│   Android iOS Win Mac               │
└─────────────────────────────────────┘
```

### 1.2 Projet MAUI - Structure

```
MyApp/
├── Platforms/           # Code spécifique par plateforme
│   ├── Android/
│   │   ├── MainActivity.cs
│   │   ├── MainApplication.cs
│   │   └── AndroidManifest.xml
│   ├── iOS/
│   │   ├── AppDelegate.cs
│   │   ├── Program.cs
│   │   └── Info.plist
│   ├── Windows/
│   │   └── App.xaml.cs
│   └── MacCatalyst/
│       └── AppDelegate.cs
├── Resources/           # Ressources partagées
│   ├── Images/
│   ├── Fonts/
│   ├── Styles/
│   └── AppIcon/
├── Views/              # Pages XAML
│   └── MainPage.xaml
├── ViewModels/         # Logique présentation
├── Models/             # Données
├── Services/           # Logique métier
├── App.xaml           # Application racine
├── AppShell.xaml      # Navigation Shell
└── MauiProgram.cs     # Point d'entrée
```

### 1.3 MauiProgram.cs - Configuration

```c#
public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();

        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        // Enregistrer services (Dependency Injection)
        builder.Services.AddSingleton<IWifiService, WifiService>();
        builder.Services.AddSingleton<ISecurityAnalysisService, SecurityAnalysisService>();

        // Enregistrer ViewModels
        builder.Services.AddTransient<MainViewModel>();
        builder.Services.AddTransient<NetworkDetailViewModel>();

        // Enregistrer Pages
        builder.Services.AddTransient<MainPage>();
        builder.Services.AddTransient<NetworkDetailPage>();

        // Configuration spécifique plateforme
#if ANDROID
        builder.Services.AddSingleton<IWifiService, AndroidWifiService>();
#elif IOS
        builder.Services.AddSingleton<IWifiService, IOSWifiService>();
#endif

        return builder.Build();
    }
}
```

### 1.4 App.xaml et App.xaml.cs

```xml
<!-- App.xaml -->
<?xml version="1.0" encoding="UTF-8" ?>
<Application xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="WiFiAuditor.App">
    <Application.Resources>
        <ResourceDictionary>
            <ResourceDictionary.MergedDictionaries>
                <ResourceDictionary Source="Resources/Styles/Colors.xaml" />
                <ResourceDictionary Source="Resources/Styles/Styles.xaml" />
            </ResourceDictionary.MergedDictionaries>
        </ResourceDictionary>
    </Application.Resources>
</Application>
```

```c#
// App.xaml.cs
public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        MainPage = new AppShell();
    }

    protected override void OnStart()
    {
        // Appelé au démarrage
    }

    protected override void OnSleep()
    {
        // Appelé quand app en arrière-plan
    }

    protected override void OnResume()
    {
        // Appelé quand app revient au premier plan
    }
}
```

---

## 2. XAML (eXtensible Application Markup Language)

### 2.1 Syntaxe XAML

```xml
<!-- Éléments de base -->
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="MyApp.MainPage"
             Title="WiFi Auditor">

    <!-- Layout racine -->
    <VerticalStackLayout Padding="20" Spacing="10">

        <!-- Label simple -->
        <Label Text="Hello World"
               FontSize="24"
               TextColor="Blue"/>

        <!-- Label avec propriété complexe -->
        <Label FontSize="18">
            <Label.FormattedText>
                <FormattedString>
                    <Span Text="Bold " FontAttributes="Bold"/>
                    <Span Text="Italic" FontAttributes="Italic"/>
                </FormattedString>
            </Label.FormattedText>
        </Label>

        <!-- Button avec événement -->
        <Button Text="Click Me"
                Clicked="OnButtonClicked"
                BackgroundColor="#512BD4"
                TextColor="White"/>

        <!-- Entry (input) -->
        <Entry Placeholder="Enter SSID"
               Text="{Binding Ssid}"
               Keyboard="Default"/>

        <!-- ListView -->
        <ListView ItemsSource="{Binding Networks}">
            <ListView.ItemTemplate>
                <DataTemplate>
                    <TextCell Text="{Binding Ssid}"
                              Detail="{Binding SecurityType}"/>
                </DataTemplate>
            </ListView.ItemTemplate>
        </ListView>

    </VerticalStackLayout>
</ContentPage>
```

### 2.2 Layouts

```xml
<!-- VerticalStackLayout - Empilement vertical -->
<VerticalStackLayout Spacing="10" Padding="20">
    <Label Text="Item 1"/>
    <Label Text="Item 2"/>
    <Label Text="Item 3"/>
</VerticalStackLayout>

<!-- HorizontalStackLayout - Empilement horizontal -->
<HorizontalStackLayout Spacing="10">
    <Label Text="Left"/>
    <Label Text="Center"/>
    <Label Text="Right"/>
</HorizontalStackLayout>

<!-- Grid - Grille -->
<Grid RowDefinitions="Auto,*,Auto"
      ColumnDefinitions="*,2*"
      RowSpacing="10"
      ColumnSpacing="10">

    <!-- Row 0, Column 0 -->
    <Label Grid.Row="0" Grid.Column="0" Text="Header Left"/>

    <!-- Row 0, Column 1 -->
    <Label Grid.Row="0" Grid.Column="1" Text="Header Right"/>

    <!-- Row 1, Column 0-1 (span 2 colonnes) -->
    <Label Grid.Row="1" Grid.Column="0" Grid.ColumnSpan="2"
           Text="Content"/>

    <!-- Row 2 -->
    <Button Grid.Row="2" Grid.Column="0" Text="Cancel"/>
    <Button Grid.Row="2" Grid.Column="1" Text="OK"/>
</Grid>

<!-- FlexLayout - Layout flexible -->
<FlexLayout Direction="Row"
            Wrap="Wrap"
            JustifyContent="SpaceBetween">
    <Label Text="Item 1"/>
    <Label Text="Item 2"/>
    <Label Text="Item 3"/>
</FlexLayout>

<!-- AbsoluteLayout - Position absolue -->
<AbsoluteLayout>
    <BoxView Color="Blue"
             AbsoluteLayout.LayoutBounds="0,0,100,100"/>
    <Label Text="Centered"
           AbsoluteLayout.LayoutBounds="0.5,0.5,AutoSize,AutoSize"
           AbsoluteLayout.LayoutFlags="PositionProportional"/>
</AbsoluteLayout>
```

### 2.3 Data Binding

```xml
<!-- ContentPage avec BindingContext -->
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:viewmodels="clr-namespace:MyApp.ViewModels"
             x:Class="MyApp.MainPage"
             x:DataType="viewmodels:MainViewModel">

    <VerticalStackLayout>

        <!-- OneWay binding (lecture seule) -->
        <Label Text="{Binding NetworkCount}"/>

        <!-- TwoWay binding (lecture/écriture) -->
        <Entry Text="{Binding SearchText, Mode=TwoWay}"/>

        <!-- Binding avec converter -->
        <Label Text="{Binding IsScanning,
                      Converter={StaticResource BoolToTextConverter}}"/>

        <!-- Binding sur Command -->
        <Button Text="Scan"
                Command="{Binding ScanCommand}"/>

        <!-- Binding dans ItemTemplate -->
        <CollectionView ItemsSource="{Binding Networks}">
            <CollectionView.ItemTemplate>
                <DataTemplate x:DataType="models:WifiNetwork">
                    <Label Text="{Binding Ssid}"/>
                </DataTemplate>
            </CollectionView.ItemTemplate>
        </CollectionView>

    </VerticalStackLayout>
</ContentPage>
```

```c#
// Code-behind
public partial class MainPage : ContentPage
{
    public MainPage(MainViewModel viewModel)
    {
        InitializeComponent();

        // Définir BindingContext
        BindingContext = viewModel;
    }
}
```

### 2.4 Value Converters

```c#
// Converter Bool → String
public class BoolToTextConverter : IValueConverter
{
    public object Convert(object value, Type targetType,
        object parameter, CultureInfo culture)
    {
        if (value is bool boolValue)
        {
            return boolValue ? "En cours..." : "Prêt";
        }
        return "Inconnu";
    }

    public object ConvertBack(object value, Type targetType,
        object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

// Enregistrer dans App.xaml
<Application.Resources>
    <ResourceDictionary>
        <local:BoolToTextConverter x:Key="BoolToTextConverter"/>
    </ResourceDictionary>
</Application.Resources>
```

### 2.5 Styles et Resources

```xml
<!-- Resources/Styles/Colors.xaml -->
<ResourceDictionary xmlns="http://schemas.microsoft.com/dotnet/2021/maui">
    <Color x:Key="Primary">#512BD4</Color>
    <Color x:Key="Secondary">#DFD8F7</Color>
    <Color x:Key="Success">#4CAF50</Color>
    <Color x:Key="Warning">#FF9800</Color>
    <Color x:Key="Danger">#F44336</Color>
</ResourceDictionary>

<!-- Resources/Styles/Styles.xaml -->
<ResourceDictionary xmlns="http://schemas.microsoft.com/dotnet/2021/maui">

    <!-- Style pour Label -->
    <Style TargetType="Label">
        <Setter Property="TextColor" Value="{StaticResource Gray900}"/>
        <Setter Property="FontFamily" Value="OpenSansRegular"/>
        <Setter Property="FontSize" Value="14"/>
    </Style>

    <!-- Style nommé -->
    <Style x:Key="TitleLabel" TargetType="Label">
        <Setter Property="FontSize" Value="24"/>
        <Setter Property="FontAttributes" Value="Bold"/>
        <Setter Property="TextColor" Value="{StaticResource Primary}"/>
    </Style>

    <!-- Style hérité -->
    <Style x:Key="SubtitleLabel"
           TargetType="Label"
           BasedOn="{StaticResource TitleLabel}">
        <Setter Property="FontSize" Value="18"/>
    </Style>

    <!-- Style pour Button -->
    <Style TargetType="Button">
        <Setter Property="BackgroundColor" Value="{StaticResource Primary}"/>
        <Setter Property="TextColor" Value="White"/>
        <Setter Property="CornerRadius" Value="10"/>
        <Setter Property="Padding" Value="15"/>
    </Style>

</ResourceDictionary>
```

```xml
<!-- Utilisation dans page -->
<Label Text="Titre" Style="{StaticResource TitleLabel}"/>
<Label Text="Sous-titre" Style="{StaticResource SubtitleLabel}"/>
```

---

## 3. Navigation

### 3.1 Shell Navigation

```xml
<!-- AppShell.xaml -->
<?xml version="1.0" encoding="UTF-8" ?>
<Shell xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
       xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
       xmlns:views="clr-namespace:MyApp.Views"
       x:Class="MyApp.AppShell">

    <!-- Tabs en bas -->
    <TabBar>
        <Tab Title="Scan" Icon="wifi.png">
            <ShellContent ContentTemplate="{DataTemplate views:MainPage}"/>
        </Tab>

        <Tab Title="Éducation" Icon="book.png">
            <ShellContent ContentTemplate="{DataTemplate views:EducationalPage}"/>
        </Tab>

        <Tab Title="Paramètres" Icon="settings.png">
            <ShellContent ContentTemplate="{DataTemplate views:SettingsPage}"/>
        </Tab>
    </TabBar>

</Shell>
```

```c#
// AppShell.xaml.cs - Enregistrer routes
public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        // Enregistrer routes pour navigation
        Routing.RegisterRoute("networkdetail", typeof(NetworkDetailPage));
        Routing.RegisterRoute("educational/content", typeof(ContentDetailPage));
    }
}

// Navigation vers une page
public class NavigationExamples
{
    public async Task NavigateExamples()
    {
        // Navigation simple
        await Shell.Current.GoToAsync("networkdetail");

        // Navigation avec paramètres
        await Shell.Current.GoToAsync($"networkdetail?ssid={ssid}&bssid={bssid}");

        // Navigation absolue
        await Shell.Current.GoToAsync("//MainPage/networkdetail");

        // Navigation arrière
        await Shell.Current.GoToAsync("..");

        // Navigation vers route
        await Shell.Current.GoToAsync("educational/content");
    }
}

// Page avec paramètres
[QueryProperty(nameof(Ssid), "ssid")]
[QueryProperty(nameof(Bssid), "bssid")]
public partial class NetworkDetailPage : ContentPage
{
    private string ssid;
    private string bssid;

    public string Ssid
    {
        get => ssid;
        set
        {
            ssid = Uri.UnescapeDataString(value ?? string.Empty);
            LoadData();
        }
    }

    public string Bssid
    {
        get => bssid;
        set => bssid = Uri.UnescapeDataString(value ?? string.Empty);
    }

    private void LoadData()
    {
        // Charger données basées sur ssid/bssid
    }
}
```

### 3.2 Navigation Traditionnelle (sans Shell)

```c#
public class TraditionalNavigation
{
    // Push (empiler)
    public async Task PushPage()
    {
        var detailPage = new NetworkDetailPage();
        await Navigation.PushAsync(detailPage);
    }

    // Pop (dépiler)
    public async Task PopPage()
    {
        await Navigation.PopAsync();
    }

    // Modal
    public async Task ShowModal()
    {
        var modalPage = new SettingsPage();
        await Navigation.PushModalAsync(modalPage);
    }

    public async Task CloseModal()
    {
        await Navigation.PopModalAsync();
    }
}
```

---

## 4. Platform-Specific Code

### 4.1 Directives de Compilation

```c#
public class PlatformSpecific
{
    public void Example()
    {
#if ANDROID
        // Code Android uniquement
        var wifiManager = (WifiManager)Android.App.Application.Context
            .GetSystemService(Context.WifiService);
#elif IOS
        // Code iOS uniquement
        var config = new NEHotspotConfiguration();
#elif WINDOWS
        // Code Windows uniquement
        var wifiAdapter = await WiFiAdapter.FindAllAdaptersAsync();
#endif
    }

    // Méthode partielle (définie par plateforme)
    partial void InitializePlatform();

    public void Initialize()
    {
        InitializePlatform();
    }
}

// Platforms/Android/PlatformSpecific.cs
public partial class PlatformSpecific
{
    partial void InitializePlatform()
    {
        // Initialisation Android
    }
}

// Platforms/iOS/PlatformSpecific.cs
public partial class PlatformSpecific
{
    partial void InitializePlatform()
    {
        // Initialisation iOS
    }
}
```

### 4.2 Dependency Injection

```c#
// Interface commune
public interface IWifiService
{
    Task<List<WifiNetwork>> ScanNetworksAsync();
}

// Implémentation Android
#if ANDROID
public class AndroidWifiService : IWifiService
{
    public async Task<List<WifiNetwork>> ScanNetworksAsync()
    {
        // Code Android
    }
}
#endif

// Implémentation iOS
#if IOS
public class IOSWifiService : IWifiService
{
    public async Task<List<WifiNetwork>> ScanNetworksAsync()
    {
        // Code iOS
    }
}
#endif

// Enregistrement dans MauiProgram.cs
builder.Services.AddSingleton<IWifiService>(serviceProvider =>
{
#if ANDROID
    return new AndroidWifiService();
#elif IOS
    return new IOSWifiService();
#else
    return new DummyWifiService();
#endif
});
```

---

## 5. Permissions

### 5.1 Déclarer Permissions

```xml
<!-- Platforms/Android/AndroidManifest.xml -->
<manifest>
    <uses-permission android:name="android.permission.ACCESS_WIFI_STATE" />
    <uses-permission android:name="android.permission.CHANGE_WIFI_STATE" />
    <uses-permission android:name="android.permission.ACCESS_FINE_LOCATION" />
    <uses-permission android:name="android.permission.ACCESS_COARSE_LOCATION" />
</manifest>
```

```xml
<!-- Platforms/iOS/Info.plist -->
<key>NSLocationWhenInUseUsageDescription</key>
<string>Nous avons besoin de votre localisation pour scanner les réseaux WiFi</string>
```

### 5.2 Demander Permissions Runtime

```c#
public class PermissionsExample
{
    public async Task<bool> RequestLocationPermissionAsync()
    {
        // Vérifier status actuel
        var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();

        if (status == PermissionStatus.Granted)
            return true;

        if (status == PermissionStatus.Denied && DeviceInfo.Platform == DevicePlatform.iOS)
        {
            // Sur iOS, si denied, rediriger vers paramètres
            await DisplayAlert("Permission requise",
                "Veuillez activer la localisation dans les paramètres",
                "OK");
            return false;
        }

        // Demander permission
        status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();

        return status == PermissionStatus.Granted;
    }

    // Permission personnalisée
    public class WifiPermission : Permissions.BasePermission
    {
        public override (string androidPermission, bool isRuntime)[] RequiredPermissions =>
            new List<(string, bool)>
            {
                (Android.Manifest.Permission.AccessWifiState, true),
                (Android.Manifest.Permission.ChangeWifiState, true),
                (Android.Manifest.Permission.AccessFineLocation, true)
            }.ToArray();
    }

    public async Task<bool> RequestWifiPermissionAsync()
    {
        var status = await Permissions.CheckStatusAsync<WifiPermission>();
        if (status != PermissionStatus.Granted)
        {
            status = await Permissions.RequestAsync<WifiPermission>();
        }
        return status == PermissionStatus.Granted;
    }
}
```

---

# PARTIE IV - ARCHITECTURE MVVM

## 1. Qu'est-ce que MVVM ?

**Model-View-ViewModel**

```
┌──────────┐         ┌──────────────┐         ┌───────┐
│  View    │ ◄─────► │  ViewModel   │ ◄─────► │ Model │
│  (XAML)  │ Binding │  (Logic)     │  Uses   │ (Data)│
└──────────┘         └──────────────┘         └───────┘
```

**Responsabilités :**

**Model** :
- Données métier
- Logique métier
- Pas de référence à View ou ViewModel

**ViewModel** :
- Logique de présentation
- Commands pour actions utilisateur
- INotifyPropertyChanged pour mise à jour UI
- Pas de référence directe aux contrôles UI

**View** :
- Interface utilisateur (XAML)
- Data binding vers ViewModel
- Logique UI minimale (code-behind)

---

## 2. Implémentation MVVM

### 2.1 ObservableObject (Base ViewModel)

```c#
public class ObservableObject : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;

    // Méthode pour notifier changement
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    // Helper pour set property avec notification
    protected bool SetProperty<T>(
        ref T field,
        T value,
        [CallerMemberName] string propertyName = null)
    {
        // Vérifier si valeur a changé
        if (EqualityComparer<T>.Default.Equals(field, value))
            return false;

        // Mettre à jour champ
        field = value;

        // Notifier changement
        OnPropertyChanged(propertyName);

        return true;
    }

    // Set property avec callback
    protected bool SetProperty<T>(
        ref T field,
        T value,
        Action onChanged,
        [CallerMemberName] string propertyName = null)
    {
        if (SetProperty(ref field, value, propertyName))
        {
            onChanged?.Invoke();
            return true;
        }
        return false;
    }
}
```

### 2.2 RelayCommand (ICommand)

```c#
public class RelayCommand : ICommand
{
    private readonly Action<object> _execute;
    private readonly Func<object, bool> _canExecute;

    public event EventHandler CanExecuteChanged;

    public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
    {
        _execute = execute ?? throw new ArgumentNullException(nameof(execute));
        _canExecute = canExecute;
    }

    // Constructeur sans paramètre
    public RelayCommand(Action execute, Func<bool> canExecute = null)
        : this(
            execute: _ => execute(),
            canExecute: canExecute != null ? _ => canExecute() : null)
    {
    }

    public bool CanExecute(object parameter)
    {
        return _canExecute?.Invoke(parameter) ?? true;
    }

    public void Execute(object parameter)
    {
        _execute(parameter);
    }

    // Forcer réévaluation de CanExecute
    public void RaiseCanExecuteChanged()
    {
        CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}

// Async variant
public class AsyncRelayCommand : ICommand
{
    private readonly Func<Task> _execute;
    private readonly Func<bool> _canExecute;
    private bool _isExecuting;

    public event EventHandler CanExecuteChanged;

    public AsyncRelayCommand(Func<Task> execute, Func<bool> canExecute = null)
    {
        _execute = execute;
        _canExecute = canExecute;
    }

    public bool CanExecute(object parameter)
    {
        return !_isExecuting && (_canExecute?.Invoke() ?? true);
    }

    public async void Execute(object parameter)
    {
        if (CanExecute(parameter))
        {
            try
            {
                _isExecuting = true;
                RaiseCanExecuteChanged();

                await _execute();
            }
            finally
            {
                _isExecuting = false;
                RaiseCanExecuteChanged();
            }
        }
    }

    public void RaiseCanExecuteChanged()
    {
        CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}
```

### 2.3 ViewModel Complet

```c#
public class MainViewModel : ObservableObject
{
    private readonly IWifiService _wifiService;
    private readonly ISecurityAnalysisService _securityService;

    // Champs privés (backing fields)
    private ObservableCollection<WifiNetwork> _networks;
    private WifiNetwork _selectedNetwork;
    private bool _isScanning;
    private string _statusMessage;

    // Constructeur avec injection de dépendances
    public MainViewModel(
        IWifiService wifiService,
        ISecurityAnalysisService securityService)
    {
        _wifiService = wifiService;
        _securityService = securityService;

        // Initialiser collection
        Networks = new ObservableCollection<WifiNetwork>();

        // Initialiser commands
        ScanCommand = new AsyncRelayCommand(
            execute: ScanNetworksAsync,
            canExecute: () => !IsScanning);

        RefreshCommand = new AsyncRelayCommand(
            execute: ScanNetworksAsync,
            canExecute: () => !IsScanning);

        AnalyzeCommand = new AsyncRelayCommand(
            execute: AnalyzeSelectedNetworkAsync,
            canExecute: () => SelectedNetwork != null);
    }

    // Properties avec notification
    public ObservableCollection<WifiNetwork> Networks
    {
        get => _networks;
        set => SetProperty(ref _networks, value);
    }

    public WifiNetwork SelectedNetwork
    {
        get => _selectedNetwork;
        set
        {
            if (SetProperty(ref _selectedNetwork, value))
            {
                // Mettre à jour état des commands
                ((AsyncRelayCommand)AnalyzeCommand).RaiseCanExecuteChanged();
            }
        }
    }

    public bool IsScanning
    {
        get => _isScanning;
        set
        {
            if (SetProperty(ref _isScanning, value))
            {
                // Mettre à jour état des commands
                ((AsyncRelayCommand)ScanCommand).RaiseCanExecuteChanged();
                ((AsyncRelayCommand)RefreshCommand).RaiseCanExecuteChanged();
            }
        }
    }

    public string StatusMessage
    {
        get => _statusMessage;
        set => SetProperty(ref _statusMessage, value);
    }

    // Commands
    public ICommand ScanCommand { get; }
    public ICommand RefreshCommand { get; }
    public ICommand AnalyzeCommand { get; }

    // Méthodes métier
    private async Task ScanNetworksAsync()
    {
        try
        {
            IsScanning = true;
            StatusMessage = "Scan en cours...";

            // Vérifier permissions
            var hasPermission = await _wifiService.RequestLocationPermissionAsync();
            if (!hasPermission)
            {
                StatusMessage = "Permission requise";
                return;
            }

            // Scanner
            var networks = await _wifiService.ScanNetworksAsync();

            // Trier par signal
            var sorted = networks.OrderByDescending(n => n.SignalStrength).ToList();

            // Mettre à jour collection
            Networks.Clear();
            foreach (var network in sorted)
            {
                Networks.Add(network);
            }

            StatusMessage = $"{networks.Count} réseau(x) trouvé(s)";
        }
        catch (Exception ex)
        {
            StatusMessage = $"Erreur: {ex.Message}";
        }
        finally
        {
            IsScanning = false;
        }
    }

    private async Task AnalyzeSelectedNetworkAsync()
    {
        if (SelectedNetwork == null) return;

        try
        {
            StatusMessage = "Analyse en cours...";

            var analysis = await _securityService.AnalyzeNetworkAsync(SelectedNetwork);

            // Navigation ou affichage résultats
            // (dépend de l'implémentation)

            StatusMessage = "Analyse terminée";
        }
        catch (Exception ex)
        {
            StatusMessage = $"Erreur: {ex.Message}";
        }
    }
}
```

### 2.4 View avec Binding

```xml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:viewmodels="clr-namespace:MyApp.ViewModels"
             x:Class="MyApp.Views.MainPage"
             x:DataType="viewmodels:MainViewModel"
             Title="WiFi Auditor">

    <Grid RowDefinitions="Auto,*,Auto">

        <!-- Header -->
        <VerticalStackLayout Grid.Row="0" Padding="20">
            <Label Text="{Binding StatusMessage}" FontSize="16"/>
            <ActivityIndicator IsRunning="{Binding IsScanning}"
                               IsVisible="{Binding IsScanning}"/>
        </VerticalStackLayout>

        <!-- Liste réseaux -->
        <CollectionView Grid.Row="1"
                        ItemsSource="{Binding Networks}"
                        SelectionMode="Single"
                        SelectedItem="{Binding SelectedNetwork}">
            <CollectionView.ItemTemplate>
                <DataTemplate x:DataType="models:WifiNetwork">
                    <Frame Padding="15" Margin="10,5">
                        <Grid ColumnDefinitions="*,Auto">
                            <VerticalStackLayout Grid.Column="0">
                                <Label Text="{Binding Ssid}"
                                       FontAttributes="Bold"
                                       FontSize="18"/>
                                <Label Text="{Binding SecurityType}"
                                       FontSize="14"/>
                                <Label Text="{Binding SignalQuality}"
                                       FontSize="12"/>
                            </VerticalStackLayout>

                            <Label Grid.Column="1"
                                   Text="{Binding SecurityScore}"
                                   FontSize="32"
                                   FontAttributes="Bold"/>
                        </Grid>
                    </Frame>
                </DataTemplate>
            </CollectionView.ItemTemplate>
        </CollectionView>

        <!-- Boutons -->
        <Grid Grid.Row="2"
              ColumnDefinitions="*,*"
              ColumnSpacing="10"
              Padding="20">
            <Button Grid.Column="0"
                    Text="Scanner"
                    Command="{Binding ScanCommand}"/>
            <Button Grid.Column="1"
                    Text="Analyser"
                    Command="{Binding AnalyzeCommand}"/>
        </Grid>

    </Grid>
</ContentPage>
```

```c#
// Code-behind
public partial class MainPage : ContentPage
{
    public MainPage(MainViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
```

---

# PARTIE V - CONCEPTS AVANCÉS

## 1. Sécurité des Applications

### 1.1 Validation des Entrées

```c#
public class InputValidation
{
    // Validation SSID
    public bool IsValidSSID(string ssid)
    {
        if (string.IsNullOrWhiteSpace(ssid))
            return false;

        // SSID max 32 octets
        if (Encoding.UTF8.GetByteCount(ssid) > 32)
            return false;

        // Caractères valides
        return ssid.All(c => c >= 32 && c <= 126); // ASCII imprimables
    }

    // Validation BSSID (adresse MAC)
    public bool IsValidBSSID(string bssid)
    {
        // Format: XX:XX:XX:XX:XX:XX
        var regex = new Regex(@"^([0-9A-Fa-f]{2}:){5}[0-9A-Fa-f]{2}$");
        return regex.IsMatch(bssid);
    }

    // Sanitization pour affichage
    public string SanitizeForDisplay(string input)
    {
        if (string.IsNullOrEmpty(input))
            return string.Empty;

        // Échapper caractères spéciaux
        return System.Security.SecurityElement.Escape(input);
    }

    // Validation canal WiFi
    public bool IsValidChannel(int channel, bool is5GHz = false)
    {
        if (is5GHz)
        {
            // Canaux 5 GHz
            var validChannels = new[] { 36, 40, 44, 48, 52, 56, 60, 64,
                100, 104, 108, 112, 116, 120, 124, 128, 132, 136, 140, 144,
                149, 153, 157, 161, 165 };
            return validChannels.Contains(channel);
        }
        else
        {
            // Canaux 2.4 GHz
            return channel >= 1 && channel <= 14;
        }
    }
}
```

### 1.2 Stockage Sécurisé

```c#
public class SecureStorage
{
    // Stockage sécurisé (utilise Keychain/Keystore)
    public async Task SaveCredentialAsync(string key, string value)
    {
        try
        {
            await Microsoft.Maui.Storage.SecureStorage.SetAsync(key, value);
        }
        catch (Exception ex)
        {
            // Gérer erreur
            Console.WriteLine($"Erreur SecureStorage: {ex.Message}");
        }
    }

    public async Task<string> GetCredentialAsync(string key)
    {
        try
        {
            return await Microsoft.Maui.Storage.SecureStorage.GetAsync(key);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur SecureStorage: {ex.Message}");
            return null;
        }
    }

    // Preferences (pour données non-sensibles)
    public void SavePreference(string key, string value)
    {
        Preferences.Set(key, value);
    }

    public string GetPreference(string key, string defaultValue = "")
    {
        return Preferences.Get(key, defaultValue);
    }

    // JAMAIS stocker mots de passe en clair
    public async Task SavePasswordHashAsync(string password)
    {
        // Hacher avec algorithme fort
        using var sha256 = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(password);
        var hash = sha256.ComputeHash(bytes);
        var hashString = Convert.ToBase64String(hash);

        await SaveCredentialAsync("password_hash", hashString);
    }
}
```

### 1.3 Logging Sécurisé

```c#
public class SecureLogging
{
    // MAUVAIS - Exposition de données sensibles
    public void BadLogging(string ssid, string password)
    {
        Debug.WriteLine($"SSID: {ssid}, Password: {password}"); // ❌ DANGER
    }

    // BON - Logging sécurisé
    public void GoodLogging(string ssid)
    {
        Debug.WriteLine($"Scanning SSID: {ssid}"); // ✅ OK
        Debug.WriteLine($"Authentication attempt"); // ✅ OK (pas de détails)
    }

    // Masquer données sensibles
    public string MaskSensitiveData(string data, int visibleChars = 3)
    {
        if (string.IsNullOrEmpty(data))
            return string.Empty;

        if (data.Length <= visibleChars)
            return new string('*', data.Length);

        return data.Substring(0, visibleChars) +
               new string('*', data.Length - visibleChars);
    }

    // Logger avec niveaux
    public void Log(LogLevel level, string message, Exception ex = null)
    {
        var timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        var logMessage = $"[{timestamp}] [{level}] {message}";

        if (ex != null)
        {
            logMessage += $"\nException: {ex.GetType().Name}: {ex.Message}";
            // NE PAS logger ex.StackTrace en production (info sensible)
        }

        Debug.WriteLine(logMessage);

        // En production : envoyer à service de logging
        // (Azure App Insights, Sentry, etc.)
    }
}

public enum LogLevel
{
    Debug,
    Info,
    Warning,
    Error,
    Critical
}
```

---

## 2. Performance et Optimisation

### 2.1 Collections Virtualisées

```xml
<!-- CollectionView (virtualisé par défaut) -->
<CollectionView ItemsSource="{Binding Networks}">
    <CollectionView.ItemTemplate>
        <DataTemplate x:DataType="models:WifiNetwork">
            <!-- Template léger pour performance -->
            <Label Text="{Binding Ssid}"/>
        </DataTemplate>
    </CollectionView.ItemTemplate>
</CollectionView>
```

### 2.2 Caching

```c#
public class CachingService
{
    private Dictionary<string, (object data, DateTime expiry)> _cache = new();

    public T GetOrCache<T>(string key, Func<T> getData, TimeSpan? expiration = null)
    {
        expiration ??= TimeSpan.FromMinutes(5);

        // Vérifier cache
        if (_cache.TryGetValue(key, out var cached))
        {
            // Vérifier expiration
            if (DateTime.Now < cached.expiry)
            {
                return (T)cached.data;
            }
            else
            {
                _cache.Remove(key);
            }
        }

        // Obtenir données
        var data = getData();

        // Mettre en cache
        _cache[key] = (data, DateTime.Now + expiration.Value);

        return data;
    }

    public async Task<T> GetOrCacheAsync<T>(
        string key,
        Func<Task<T>> getData,
        TimeSpan? expiration = null)
    {
        expiration ??= TimeSpan.FromMinutes(5);

        if (_cache.TryGetValue(key, out var cached) &&
            DateTime.Now < cached.expiry)
        {
            return (T)cached.data;
        }

        var data = await getData();
        _cache[key] = (data, DateTime.Now + expiration.Value);

        return data;
    }

    public void InvalidateCache(string key)
    {
        _cache.Remove(key);
    }

    public void ClearCache()
    {
        _cache.Clear();
    }
}

// Utilisation
public class WifiService
{
    private readonly CachingService _cache = new();

    public async Task<List<WifiNetwork>> ScanNetworksAsync()
    {
        return await _cache.GetOrCacheAsync(
            "wifi_scan",
            async () =>
            {
                // Scan coûteux
                return await PerformScanAsync();
            },
            TimeSpan.FromSeconds(30)
        );
    }
}
```

### 2.3 Parallélisation

```c#
public class ParallelProcessing
{
    public async Task ProcessNetworksParallel(List<WifiNetwork> networks)
    {
        // Analyse en parallèle
        var tasks = networks.Select(network =>
            AnalyzeNetworkAsync(network)).ToList();

        var results = await Task.WhenAll(tasks);
    }

    public void ProcessCollectionParallel<T>(List<T> items, Action<T> process)
    {
        Parallel.ForEach(items, new ParallelOptions
        {
            MaxDegreeOfParallelism = Environment.ProcessorCount
        },
        item =>
        {
            process(item);
        });
    }
}
```

---

# PARTIE VI - QUESTIONS D'ENTRETIEN

## Questions Théoriques WiFi

### Q1: Expliquez la différence entre WPA2 et WPA3
**R:** WPA3 améliore WPA2 avec :
- **SAE** (Simultaneous Authentication of Equals) au lieu de PSK
- Protection contre attaques par dictionnaire offline
- Forward secrecy (compromise password n'affecte pas sessions passées)
- Chiffrement renforcé (192-bit en Enterprise)
- Protection contre downgrade attacks

### Q2: Pourquoi WPS est-il vulnérable ?
**R:** Le PIN WPS de 8 chiffres est divisé en deux parties validées séparément :
- Première moitié : 10,000 possibilités
- Seconde moitié : 1,000 possibilités
- Total : seulement 11,000 essais au lieu de 100,000,000
- Attaque Pixie Dust exploite la faiblesse du générateur de nombres aléatoires

### Q3: Qu'est-ce que le 4-way handshake ?
**R:** Processus d'échange de clés WPA/WPA2 en 4 messages :
1. AP → Client : ANonce
2. Client → AP : SNonce + MIC (dérivation PTK)
3. AP → Client : GTK chiffré + MIC
4. Client → AP : Confirmation + MIC

### Q4: Comment fonctionne une attaque par dictionnaire sur WPA2 ?
**R:**
1. Capturer le 4-way handshake
2. Pour chaque mot de passe du dictionnaire :
   - Calculer PMK = PBKDF2(password, SSID, 4096 iterations)
   - Calculer PTK à partir de PMK + nonces
   - Calculer MIC attendu
   - Comparer avec MIC capturé
3. Si correspondance → mot de passe trouvé

### Q5: Quels sont les canaux non-chevauchants en 2.4 GHz ?
**R:** Canaux 1, 6 et 11 (aux États-Unis/Europe)
- Chaque canal occupe ~22 MHz de largeur
- Espacement de 5 MHz entre canaux adjacents
- Ces 3 canaux ne se chevauchent pas

---

## Questions Techniques C#

### Q6: Différence entre classe et struct ?
**R:**
- **Classe** : Type référence (heap), héritage, nullable, égalité par référence
- **Struct** : Type valeur (stack), pas d'héritage, non nullable (sauf Nullable<T>), égalité par valeur
- Utiliser struct pour petites données immuables (< 16 bytes)

### Q7: Qu'est-ce qu'async/await ?
**R:** Programmation asynchrone non-bloquante :
- `async` : Marque méthode comme asynchrone
- `await` : Attend résultat sans bloquer thread
- Retourne `Task` ou `Task<T>`
- Libère thread pendant attente (opérations I/O, réseau)

### Q8: Expliquez LINQ
**R:** Language Integrated Query - requêtes sur collections :
- **Query syntax** : `from x in collection where ... select ...`
- **Method syntax** : `.Where().Select().OrderBy()`
- Lazy evaluation (exécution différée)
- Opérateurs : Where, Select, OrderBy, GroupBy, Join, etc.

### Q9: Qu'est-ce que INotifyPropertyChanged ?
**R:** Interface pour notifier changement de propriété :
```c#
public event PropertyChangedEventHandler PropertyChanged;
protected void OnPropertyChanged(string name)
{
    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
```
Essentiel pour data binding (MVVM) - UI se met à jour automatiquement.

### Q10: Différence entre ref et out ?
**R:**
- **ref** : Variable doit être initialisée avant appel
- **out** : Variable doit être assignée dans méthode
- Les deux passent par référence (pas de copie)

---

## Questions Architecture MVVM

### Q11: Pourquoi utiliser MVVM ?
**R:**
- **Séparation** : UI (View) séparée de logique (ViewModel)
- **Testabilité** : ViewModel testable sans UI
- **Réutilisabilité** : Même ViewModel pour différentes Views
- **Maintenabilité** : Changements UI n'affectent pas logique
- **Data Binding** : Synchronisation automatique View ↔ ViewModel

### Q12: Qu'est-ce qu'une Command ?
**R:** Implémentation d'ICommand pour actions utilisateur :
- `Execute()` : Logique à exécuter
- `CanExecute()` : Détermine si commande peut s'exécuter
- `CanExecuteChanged` : Événement quand état change
- Permet data binding de boutons vers méthodes ViewModel

### Q13: ObservableCollection vs List ?
**R:**
- **ObservableCollection** :
  - Événement `CollectionChanged` quand collection modifiée
  - UI se met à jour automatiquement
  - Utiliser pour data binding
- **List** :
  - Pas d'événements
  - UI ne se met pas à jour automatiquement
  - Utiliser pour données internes

---

## Questions .NET MAUI

### Q14: Différence entre .NET MAUI et Xamarin.Forms ?
**R:**
- **MAUI** :
  - Single project (au lieu de projets séparés)
  - Performance améliorée
  - Handlers (au lieu de Renderers)
  - Support .NET 6+
  - Blazor hybrid
- **Xamarin.Forms** :
  - Deprecated (fin support mai 2024)
  - Projets multiples (.Android, .iOS, .UWP)

### Q15: Qu'est-ce qu'un Handler ?
**R:** Nouvelle architecture de rendu dans MAUI :
- Remplace Renderers de Xamarin.Forms
- Plus performant et léger
- Mappage direct : Control MAUI → Control natif
- Découplage entre abstraction et plateforme

---

## Mise en Situation

### Scénario : "Expliquez votre approche pour scanner les réseaux WiFi sans root"

**Réponse structurée :**

1. **API natives uniquement**
   - Android : `WifiManager.getScanResults()`
   - iOS : `NEHotspotNetwork`
   - Pas de commandes système (`su`, `aircrack-ng`)

2. **Permissions**
   ```c#
   var status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
   // Nécessaire pour WiFi scan sur Android
   ```

3. **Architecture**
   - Interface `IWifiService`
   - Implémentations par plateforme
   - Injection de dépendances

4. **Code Android**
   ```c#
   WifiManager wifiManager = ...;
   if (wifiManager.startScan()) {
       var results = wifiManager.getScanResults();
   }
   ```

5. **Limitations Android 10+**
   - Mots de passe non accessibles sans root
   - Seulement lecture des réseaux sauvegardés
   - Respecte sécurité OS

6. **Conformité**
   - Disclaimer légal obligatoire
   - Documentation des limitations
   - Approche éducative

---

## Conseils pour l'Entretien

### Préparation

1. **Savoir expliquer votre projet**
   - Architecture MVVM
   - Choix techniques
   - Défis rencontrés
   - Solutions implémentées

2. **Maîtriser les concepts**
   - WiFi (WEP, WPA, WPA2, WPA3)
   - C# (async, LINQ, génériques)
   - MAUI (binding, navigation, handlers)
   - MVVM (séparation, testabilité)

3. **Préparer des exemples de code**
   - ViewModel avec binding
   - Service async
   - Gestion d'erreurs
   - Validation d'entrées

4. **Connaître les bonnes pratiques**
   - Sécurité (validation, sanitization)
   - Performance (caching, virtualization)
   - Architecture (SOLID, DRY)

### Pendant l'Entretien

1. **Réfléchir à haute voix**
   - Montrer votre raisonnement
   - Poser des questions si nécessaire
   - Discuter des alternatives

2. **Être honnête**
   - Si vous ne savez pas, dites-le
   - Expliquer comment vous chercheriez la réponse
   - Montrer votre capacité d'apprentissage

3. **Donner du contexte**
   - Expliquer le "pourquoi" pas seulement le "comment"
   - Mentionner les trade-offs
   - Discuter des implications

4. **Rester calme**
   - Prendre le temps de réfléchir
   - Ne pas paniquer si vous bloquez
   - Demander des clarifications

---

**Bonne chance pour votre concours et vos futurs entretiens techniques ! 🚀**

**Ce cours couvre 95% des questions potentielles. Relisez-le plusieurs fois et pratiquez les exemples de code.**
