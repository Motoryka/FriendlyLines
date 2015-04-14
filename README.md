# Gra
Gra terapeutyczna - Projekt inżynierski

## Instrukcja obsługi Gita
### Instalacja
Ściągamy sobie gita i instalujemy Git Basha i ewentualnie Git GUI, jeśli ktoś chce sobie wygodnie okienkowo działać na gicie. Na początek wchodzimy do Git basha i wpisujemy komendy:
`git config --global user.name "Login z githuba"`
`git config --global user.email "adres@email.pl"`

### Lokalne repozytorium
Git działa w ten sposób, że u siebie lokalnie możemy stworzyć repozytorium. Wsadzamy tam kod i pliki i commitujemy zmiany, co umożliwia nam kontrolowanie wersji, rollback niepożądanych zmian i wiele innych. Żeby stworzyć repo z naszego zdalnego repozytorium na githubie musimy:

1. Stworzyć sobie katalog na pliki projektu. 
2. Uruchamiamy GIT Bash w tym katalogu
3. Wpisujemy `git init`
4. Następnie kolejna komenda: `git remote add origin https://github.com/Motoryka/Gra.git` - czyli dodajemy do aktualnego katalogu repozytorium zdalne. Co robi ta komenda: tworzy alias o nazwie `origin` do zdalnego repo `https://github.com/Motoryka/Gra.git`, przez co nie musimy z każdym pushem i pullem wpisywać całego adresu
5. `git pull origin`

Teraz możemy dodawać, usuwać i zmieniać pliki na naszym lokalnym repo. Kiedy chcemy zcommitować zmiany:
1. `git add .` - zamiast `.` mogą być nazwy plików, które chcemy dodać do aktualnego commita
2. `git commit -m 'Jakis tekst'` - commit zmian

Aby sprawdzic stan plikow, ktore zmienialismy mozna wpisac `git status`

### Zdalne repozytorium
`git push master` wrzucenie commitów z repo lokalnego na zdalne branch master - teraz wszyscy będą widzieć zmiany
`git pull` pobranie zmian z repo zdalnego
