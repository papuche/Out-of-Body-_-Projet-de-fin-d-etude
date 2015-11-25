import sys
from pylab import *

#sys.argv[1] = Nom du participant
#sys.argv[2] = Condition
#sys.argv[3] = Fichier de resultat du patient
#exemple : py drawGraphe.py Participant1 1 "Resultats/Participant1_11-25-2015 10-14-29 AM/Participant1.txt"

filename = sys.argv[3]

f = open(filename, 'r')

username = sys.argv[1]

valueArray = []
heightArray = []

def isInList(value, heightArray):
    for height in heightArray :
        if height == value :
            return True
    return False


for line in f:
    splittedLine = line.split('\t', 15)
    condition = splittedLine[1]
    if condition == sys.argv[2] :
        height = float(splittedLine[3])
        answer = int(splittedLine[5])
        valueArray.append({'height': height, 'answer': answer})
        if isInList(height, heightArray) == False :
            heightArray.append(height)

averageArray = []
nbRepetition = len(valueArray) / len(heightArray)

for height in heightArray :
    sum = 0
    for value in valueArray:
        if value['height'] == height :
            sum = sum + value['answer']
    averageArray.append({'height': height, 'answer': sum / nbRepetition})

averageArray = sorted(averageArray, key=lambda average: average['height'])

x = []
y = []
for average in averageArray:
    x.append(average['height'])
    y.append(average['answer'])

	
plot(x, y, alpha=0.5)

title("Courbe de résultats du " + username + " pour la condition " + condition)
xlabel("Écartement des portes")
ylabel("Taux de réponses")
grid(True)
ylim([0,1])

splittedFile = filename.split('\\')

directory = ""

for i in range(0, len(splittedFile) -1) :
    directory = directory + splittedFile[i] + '\\'
	
savefig(directory + username + "_cond" + condition)

#show()	# Commenter si on ne veut pas faire l'affichage




