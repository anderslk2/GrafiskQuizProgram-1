from flask import Flask
import sys
app = Flask(__name__)

print(sys.getsizeof(int(sys.argv[1])*"a"))
print(sys.argv)
@app.route("/")
def hello():
    return "Hello World!"

if __name__ == '__main__':
   app.run(host='0.0.0.0',port='5000')