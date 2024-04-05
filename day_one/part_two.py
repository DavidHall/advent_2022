
# lines = [
# '1000',
# '2000',
# '3000',
# '',
# '4000',
# '',
# '5000',
# '6000',
# '',
# '7000',
# '8000',
# '9000',
# '',
# '10000'
# ]



with open("input.txt") as file:
    lines = file.readlines()
 
# count = 0
# # Strips the newline character

    
    s = 0
    elf_calories = []
    for line in lines: 
        if line != '\n':
            s += int(line)
        else:
            elf_calories.append(s)
            s = 0

    sorted_elves = sorted(elf_calories, reverse=True)
    top_three = sum(sorted_elves[:3])
    
    print(top_three)