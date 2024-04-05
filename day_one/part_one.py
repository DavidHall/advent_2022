
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

    max = 0
    sum = 0

    for line in lines: 
        if line != '\n':
            sum += int(line)
        else:
            if sum > max:
                max = sum
            sum = 0

    print(max)