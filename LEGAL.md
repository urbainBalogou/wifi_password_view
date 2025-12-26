# ⚖️ Cadre Légal et Éthique

## 📋 Avertissement Légal Complet

### Application de la Loi

**WiFi Security Auditor** est soumis aux lois suivantes :

#### France

**Code Pénal - Article 323-1**
> "Le fait d'accéder ou de se maintenir, frauduleusement, dans tout ou partie d'un système de traitement automatisé de données est puni de deux ans d'emprisonnement et de 60 000 € d'amende."

**Article 323-2**
> Peines aggravées en cas de :
> - Suppression ou modification de données
> - Altération du fonctionnement du système
> - Introduction de données dans le système

**Peines encourues :**
- Prison : jusqu'à 3 ans (5 ans en cas de circonstances aggravantes)
- Amende : jusqu'à 100 000€ (150 000€ en cas de circonstances aggravantes)

#### Union Européenne

**RGPD (Règlement Général sur la Protection des Données)**
- Interception de communications : violation grave
- Collecte de données personnelles sans consentement : sanctionnable
- Amendes jusqu'à 20 millions d'euros ou 4% du chiffre d'affaires mondial

#### International

**Convention de Budapest sur la Cybercriminalité**
- Ratifiée par 65+ pays
- Harmonisation des législations nationales
- Coopération internationale pour les poursuites

## ✅ Utilisations Légales

### 1. Test de Votre Propre Réseau

**Conditions :**
- Vous êtes le propriétaire du réseau WiFi
- Le matériel (routeur, points d'accès) vous appartient
- Aucune autre personne n'est affectée

**Recommandations :**
- Documentez vos tests
- Informez les personnes partageant votre connexion
- Conservez les preuves de propriété

### 2. Audit Autorisé

**Conditions OBLIGATOIRES :**

✅ **Autorisation écrite** explicite du propriétaire du réseau

L'autorisation doit inclure :
- Nom et signature du propriétaire du réseau
- Périmètre exact des tests autorisés
- Période de temps définie
- Méthodes autorisées
- Clause de non-responsabilité
- Engagement de confidentialité

**Exemple de document d'autorisation :**

```
AUTORISATION D'AUDIT DE SÉCURITÉ WIFI

Je soussigné(e), [NOM PRÉNOM], propriétaire du réseau WiFi [NOM DU RÉSEAU],
autorise [AUDITEUR] à réaliser un audit de sécurité du réseau susmentionné.

Périmètre : [Décrire précisément]
Période : Du [DATE] au [DATE]
Méthodes autorisées : [Scanner, analyse de vulnérabilités, etc.]

Je m'engage à :
- Fournir les accès nécessaires
- Ne pas tenir l'auditeur responsable des interruptions de service
- Être informé des résultats de l'audit

L'auditeur s'engage à :
- Respecter strictement le périmètre défini
- Maintenir la confidentialité des informations découvertes
- Fournir un rapport détaillé
- Ne pas divulguer les vulnérabilités à des tiers

Fait à [LIEU], le [DATE]

Signature du propriétaire :           Signature de l'auditeur :
```

### 3. Environnement Contrôlé

**Acceptable dans les cas suivants :**

- **Laboratoires de sécurité** : Infrastructure dédiée aux tests
- **Machines virtuelles** : Environnements isolés
- **Réseaux de test** : Créés spécifiquement pour l'audit
- **Plateformes CTF** : Compétitions officielles de cybersécurité

**Exemples de plateformes légales :**
- HackTheBox (https://www.hackthebox.com)
- TryHackMe (https://tryhackme.com)
- Root Me (https://www.root-me.org)
- PentesterLab (https://pentesterlab.com)

### 4. Formation et Éducation

**Dans le cadre de :**
- Cursus universitaires en cybersécurité
- Certifications professionnelles (CEH, OSCP, etc.)
- Ateliers et formations officiels
- Démonstrations éducatives

**Toujours sur :**
- Équipement personnel
- Réseaux de test dédiés
- Simulateurs

## ❌ Utilisations ILLÉGALES

### Actions Strictement Interdites

1. **Scanner des réseaux WiFi publics ou privés sans autorisation**
   - Même sans tentative de connexion
   - Simple scan = accès frauduleux

2. **Tenter de se connecter à un réseau sans autorisation**
   - Même si le réseau est ouvert
   - Même "juste pour tester"

3. **Capturer du trafic réseau**
   - Interception de communications (Article 226-15 du Code pénal)
   - Violation du secret des correspondances
   - Peines : 1 an de prison et 45 000€ d'amende

4. **Exploiter des vulnérabilités découvertes**
   - Utilisation frauduleuse (Article 323-3)
   - Peines aggravées

5. **Distribuer des outils d'attaque**
   - Fourniture de moyens (Article 323-3-1)
   - Prison et amendes lourdes

6. **Divulguer des vulnérabilités sans responsible disclosure**
   - Peut constituer une complicité
   - Mise en danger d'autrui

## 🔍 Cas Limites et Questions Fréquentes

### Q: Puis-je scanner les réseaux de mes voisins ?
**R: NON.** Absolument illégal, même sans tentative de connexion.

### Q: Et si le réseau est ouvert (sans mot de passe) ?
**R: NON.** L'absence de sécurité n'implique pas une autorisation d'accès.

### Q: Je veux juste voir si mon voisin a un réseau sécurisé pour le prévenir ?
**R: NON.** Vos intentions n'ont pas d'importance légalement. Parlez-lui directement.

### Q: Puis-je tester le WiFi d'un café / hôtel ?
**R: NON**, sauf autorisation écrite du gérant. Utiliser le service ≠ tester sa sécurité.

### Q: Et dans le cadre d'un stage / emploi en cybersécurité ?
**R: OUI**, mais uniquement :
- Avec un contrat de travail ou convention de stage
- Mission explicite dans le cadre professionnel
- Périmètre clairement défini par l'employeur
- Assurance responsabilité civile professionnelle

### Q: Puis-je participer à un bug bounty sur des infrastructures WiFi ?
**R: OUI**, si et seulement si :
- Le programme de bug bounty est officiel
- Les réseaux WiFi sont explicitement dans le scope
- Vous respectez les règles du programme
- Vous divulguez de manière responsable

## 📝 Responsible Disclosure

Si vous découvrez une vulnérabilité dans un système que vous n'êtes pas autorisé à tester :

### ❌ NE PAS

- Exploiter la vulnérabilité
- La divulguer publiquement immédiatement
- Menacer l'organisation
- Demander une rançon

### ✅ FAIRE

1. **Documenter** la vulnérabilité (sans exploiter)
2. **Contacter** le propriétaire du système de manière privée
3. **Donner un délai raisonnable** pour la correction (90 jours généralement)
4. **Coordonner** la divulgation publique
5. **Respecter** la confidentialité jusqu'à la correction

## 🏢 Utilisation Professionnelle

### Pentesters et Consultants en Sécurité

**Obligations légales :**

1. **Contrat écrit** avec le client
2. **Lettre de mission** détaillée
3. **Assurance responsabilité civile professionnelle**
4. **Respect du périmètre** strictement défini
5. **Rapport confidentiel** au client uniquement
6. **Conservation des preuves** pendant la durée légale

**Recommandations :**

- Adhérer à un code d'éthique professionnel
- Obtenir des certifications reconnues (CEH, OSCP, GPEN)
- Maintenir une veille juridique
- Former régulièrement son équipe aux aspects légaux

## 🎓 Utilisation Académique

### Étudiants et Chercheurs

**Cadre autorisé :**

1. **Projet de recherche** approuvé par l'institution
2. **Infrastructure dédiée** mise à disposition
3. **Supervision** d'un enseignant/tuteur
4. **Publication** suivant les règles éthiques de la communauté scientifique

**Comité d'éthique :**

Pour les recherches sensibles :
- Soumettre le projet à un comité d'éthique
- Obtenir l'approbation avant les tests
- Respecter les protocoles de sécurité

## ⚠️ Conséquences d'une Utilisation Illégale

### Sanctions Pénales

**Condamnations réelles en France :**

- 2019 : 6 mois de prison avec sursis pour accès non autorisé à un WiFi
- 2020 : 18 mois de prison ferme pour exploitation de failles WiFi
- 2021 : 100 000€ d'amende pour un chercheur ayant divulgué sans autorisation

### Conséquences Civiles

- **Dommages et intérêts** à la victime
- **Frais de justice**
- **Réparation du préjudice**

### Conséquences Professionnelles

- **Interdiction d'exercer** dans le domaine informatique
- **Inscription au casier judiciaire**
- **Perte d'emploi**
- **Impossibilité d'obtenir certaines certifications**

### Conséquences Académiques

- **Exclusion** de l'établissement
- **Annulation** du diplôme
- **Poursuites disciplinaires**

## 📞 En Cas de Doute

**Avant d'utiliser cet outil, demandez-vous :**

1. Suis-je le propriétaire légitime du réseau ?
2. Ai-je une autorisation écrite explicite ?
3. Suis-je dans un environnement de test contrôlé ?
4. Mon utilisation est-elle strictement éducative sur mon matériel ?

**Si vous répondez NON à toutes ces questions : N'UTILISEZ PAS CET OUTIL.**

**En cas de doute :**
- Consultez un avocat spécialisé en droit informatique
- Contactez l'ANSSI (Agence Nationale de la Sécurité des Systèmes d'Information)
- Référez-vous aux associations professionnelles (CLUSIF, CESIN)

## 📚 Ressources Complémentaires

### Textes de Loi

- Code pénal français : https://www.legifrance.gouv.fr
- Convention de Budapest : https://www.coe.int/en/web/cybercrime
- RGPD : https://www.cnil.fr

### Organismes de Référence

- **ANSSI** (France) : https://www.ssi.gouv.fr
- **CNIL** : https://www.cnil.fr
- **CLUSIF** : https://clusif.fr

### Codes d'Éthique

- (ISC)² Code of Ethics
- EC-Council Code of Ethics
- ACM Code of Ethics and Professional Conduct

---

**Dernière mise à jour :** Décembre 2025

**Disclaimer :** Ce document est fourni à titre informatif uniquement et ne constitue pas un conseil juridique. Consultez un avocat pour des questions spécifiques à votre situation.
