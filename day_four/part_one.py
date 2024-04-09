'''
take line
make section1, seaction2
if (section1[0] < section2[0] AND section1[2] > section2[2]) OR 
(section2[0] < section1[0] AND section2[2] > section1[2]):
    sum += 1


lines = [
    '2-4,6-8',
    '2-3,4-5',
    '5-7,7-9',
    '2-8,3-7',
    '6-6,4-6',
    '2-6,4-8'
]
'''

with open('input.txt') as file:
    lines = file.readlines()

    sum = 0

    for line in lines:
        split_line = line.split(',')
        section1 = split_line[0]
        section2a = split_line[1]
        section2 = section2a[:-1]
        section1_split = section1.split('-')
        section2_split = section2.split('-')
        num1_1 = section1_split[0]
        num1_2 = section1_split[1]
        num2_1 = section2_split[0]
        num2_2 = section2_split[1]
        if (int(num1_1) <= int(num2_1) and int(num1_2) >= int(num2_2)) or (int(num2_1) <= int(num1_1) and int(num2_2) >= int(num1_2)):
            sum += 1
        
    print(sum)